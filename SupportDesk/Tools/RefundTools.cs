using SupportDesk.Data;
using SupportDesk.Data.Entities;
using SupportDesk.Http;
using System.ComponentModel;

namespace SupportDesk.Tools
{

	//public sealed class RefundTools(RefundService refunds, SupportDeskDbContext db, CustomerContext customer)
	//{
	//	[Description("Requests a refund for ONE order item. Only call this after the customer clearly asks for a refund " +
	//				 "and has confirmed which item and why. The system checks eligibility and decides by itself whether a human " +
	//				 "must approve it. Report the returned Outcome exactly and never promise that a refund will be approved.")]
	//	public Task<RefundRequestResult> RequestRefund(
	//		[Description("The order item id, taken from GetOrderDetails. Not the order number.")] int orderItemId,
	//		[Description("The customer's reason, in a few words.")] string reason,
	//		CancellationToken cancellationToken) =>
	//		refunds.RequestRefundAsync(orderItemId, reason, cancellationToken);

	//	[Description("Gets the refunds for one of the customer's orders, including whether a refund is waiting for human approval.")]
	//	public async Task<IReadOnlyList<RefundStatusView>> GetRefundStatus(
	//		[Description("The order number, for example 7001.")] int orderId,
	//		CancellationToken cancellationToken)
	//	{
	//		var rows = await db.Refunds.AsNoTracking()
	//			.Where(r => r.OrderId == orderId && r.Order.CustomerId == customer.GetCustomerId())
	//			.OrderByDescending(r => r.CreatedAt)
	//			.Select(r => new
	//			{
	//				r.RefundId,
	//				r.Amount,
	//				r.Status,
	//				r.RequiresApproval,
	//				r.CreatedAt,
	//				r.ProcessedAt,
	//				LatestApproval = r.Approvals.OrderByDescending(a => a.ApprovalId).Select(a => (ApprovalStatus?)a.Status).FirstOrDefault(),
	//			})
	//			.ToListAsync(cancellationToken);

	//		// Approver names and comments are never returned to the customer-facing agent.
	//		return rows.Select(r => new RefundStatusView(
	//			r.RefundId, r.Amount, r.Status.ToString(), r.RequiresApproval,
	//			r.LatestApproval?.ToString(), r.CreatedAt, r.ProcessedAt)).ToList();
	//	}
	//}

}
