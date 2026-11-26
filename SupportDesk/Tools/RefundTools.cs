using SupportDesk.Data;
using SupportDesk.Data.Entities;
using SupportDesk.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace SupportDesk.Tools
{
	public sealed record RefundRequestResult(
		int RefundId,
		int OrderId,
		decimal Amount,
		string Status,
		bool RequiresApproval,
		string Scope,
		string Message);

	public sealed class RefundTools
	{
		private readonly SupportDeskDbContext dbContext;
		private readonly ICustomerContext customer;

		public RefundTools(SupportDeskDbContext dbContext, ICustomerContext customer)
		{
			this.dbContext = dbContext;
			this.customer = customer;
		}

		[Description("Creates a refund request for the entire order for the signed-in customer. Use when user asks for full order refund.")]
		public async Task<RefundRequestResult> RequestFullOrderRefundAsync(
			[Description("The numeric order ID (for example 1042).")]
			int orderId,
			[Description("Reason for refund request.")]
			string reason,
			CancellationToken cancellationToken)
		{
			var order = await dbContext.Orders
				.Include(o => o.Items)
				.ThenInclude(i => i.Product)
				.FirstOrDefaultAsync(
					o => o.CustomerId == customer.GetCustomerId() && o.OrderId == orderId,
					cancellationToken);

			if (order is null)
				throw new ArgumentException($"Order {orderId} not found for the signed-in customer.");

			if (order.Items.Count == 0)
				throw new InvalidOperationException("Order has no items to refund.");

			if (order.Items.Any(i => !i.Product.IsReturnable))
				throw new InvalidOperationException("One or more items in this order are not returnable.");

			if (order.Items.Any(i => DateTime.UtcNow > order.OrderDate.AddDays(i.Product.ReturnWindowDays)))
				throw new InvalidOperationException("Return window has expired for one or more items in this order.");

			var payment = await dbContext.Set<Payment>()
				.FirstOrDefaultAsync(p => p.OrderId == orderId, cancellationToken);

			if (payment is null)
				throw new InvalidOperationException($"No payment found for order {orderId}.");

			var amount = order.TotalAmount;
			await EnsureRefundAmountAvailableAsync(orderId, amount, cancellationToken);

			var refund = new Refund
			{
				OrderId = orderId,
				PaymentId = payment.PaymentId,
				Amount = amount,
				Reason = string.IsNullOrWhiteSpace(reason) ? "Customer requested full order refund." : reason,
				Status = RefundStatus.Pending,
				RequiresApproval = true,
				CreatedAt = DateTime.UtcNow
			};

			dbContext.Refunds.Add(refund);
			dbContext.Set<RefundApproval>().Add(new RefundApproval
			{
				Refund = refund,
				Status = ApprovalStatus.Pending,
				RequestedAt = DateTime.UtcNow
			});

			await dbContext.SaveChangesAsync(cancellationToken);

			return new RefundRequestResult(
				refund.RefundId,
				orderId,
				amount,
				refund.Status.ToString(),
				refund.RequiresApproval,
				"FullOrder",
				"Refund request created for the entire order and sent for approval.");
		}

		[Description("Creates a refund request for one item in an order for the signed-in customer. Use when user asks for partial refund of a specific line item.")]
		public async Task<RefundRequestResult> RequestOrderItemRefundAsync(
			[Description("The numeric order ID (for example 1042).")]
			int orderId,
			[Description("The order item ID from that order.")]
			int orderItemId,
			[Description("Quantity of that item to refund.")]
			int quantity,
			[Description("Reason for refund request.")]
			string reason,
			CancellationToken cancellationToken)
		{
			if (quantity <= 0)
				throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

			var order = await dbContext.Orders
				.Include(o => o.Items)
				.ThenInclude(i => i.Product)
				.FirstOrDefaultAsync(
					o => o.CustomerId == customer.GetCustomerId() && o.OrderId == orderId,
					cancellationToken);

			if (order is null)
				throw new ArgumentException($"Order {orderId} not found for the signed-in customer.");

			var orderItem = order.Items.FirstOrDefault(i => i.OrderItemId == orderItemId);
			if (orderItem is null)
				throw new ArgumentException($"Order item {orderItemId} not found for order {orderId}.");

			if (!orderItem.Product.IsReturnable)
				throw new InvalidOperationException($"{orderItem.Product.Name} is not returnable.");

			if (DateTime.UtcNow > order.OrderDate.AddDays(orderItem.Product.ReturnWindowDays))
				throw new InvalidOperationException("Return window has expired for this item.");

			if (quantity > orderItem.Quantity)
				throw new InvalidOperationException($"Refund quantity cannot exceed purchased quantity ({orderItem.Quantity}).");

			var payment = await dbContext.Set<Payment>()
				.FirstOrDefaultAsync(p => p.OrderId == orderId, cancellationToken);

			if (payment is null)
				throw new InvalidOperationException($"No payment found for order {orderId}.");

			var amount = orderItem.UnitPrice * quantity;
			await EnsureRefundAmountAvailableAsync(orderId, amount, cancellationToken);

			var refund = new Refund
			{
				OrderId = orderId,
				PaymentId = payment.PaymentId,
				Amount = amount,
				Reason = string.IsNullOrWhiteSpace(reason) ? "Customer requested item-level refund." : reason,
				Status = RefundStatus.Pending,
				RequiresApproval = true,
				CreatedAt = DateTime.UtcNow
			};

			dbContext.Refunds.Add(refund);
			dbContext.Set<RefundApproval>().Add(new RefundApproval
			{
				Refund = refund,
				Status = ApprovalStatus.Pending,
				RequestedAt = DateTime.UtcNow
			});

			await dbContext.SaveChangesAsync(cancellationToken);

			return new RefundRequestResult(
				refund.RefundId,
				orderId,
				amount,
				refund.Status.ToString(),
				refund.RequiresApproval,
				"OrderItem",
				$"Refund request created for item {orderItemId} and sent for approval.");
		}

		private async Task EnsureRefundAmountAvailableAsync(int orderId, decimal requestedAmount, CancellationToken cancellationToken)
		{
			var order = await dbContext.Orders
				.AsNoTracking()
				.FirstAsync(o => o.OrderId == orderId, cancellationToken);

			var alreadyRequestedAmount = await dbContext.Refunds
				.Where(r => r.OrderId == orderId && r.Status != RefundStatus.Rejected)
				.SumAsync(r => (decimal?)r.Amount, cancellationToken) ?? 0m;

			if (alreadyRequestedAmount + requestedAmount > order.TotalAmount)
				throw new InvalidOperationException("Refund request exceeds the remaining refundable amount for this order.");
		}
	}

}
