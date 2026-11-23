using System.ComponentModel;
using SupportDesk.Data.Entities;
using SupportDesk.Data.Store;
using SupportDesk.Http;

namespace SupportDesk.Tools;

public sealed record OrderSummary(
	string OrderId, DateTimeOffset PlacedAt, string Status, decimal Total, int ItemCount, List<ProductSummary> Products);

public sealed record ProductSummary(
		string ProductName, decimal Price, int Quantity, string Category);

public sealed class OrderTools(IOrderStore orderStore, ICustomerContext customer)
{
	[Description("Returns the signed-in customer's most recent orders in descending order by placement date, including order totals, item counts, and a summarized list of products for each order.")]
	public async Task<IReadOnlyList<OrderSummary>> GetRecentOrdersAsync(
		[Description("How many orders to return, between 1 and 10.")] int count,
		CancellationToken ct)
	{
		if (count is < 1 or > 10)
			throw new ArgumentOutOfRangeException(nameof(count));

		int customerId =
			customer.GetCustomerId();

		var orders =
			await orderStore.GetRecentOrderByCustomerAsync(customerId, count, ct);

		return orders.Select(
							  x => new OrderSummary(
									x.OrderId.ToString(),
									x.OrderDate,
									x.Status.ToString(),
									x.TotalAmount,
									x.Items.Count,
									x.Items.Select(i => new ProductSummary(
										i.Product.Name,
										i.Product.Price,
										i.Quantity,
										i.Product.Category.ToString()))
										.ToList()))
					.ToList();
	}


	[Description("Returns complete details for a specific order that belongs to the signed-in customer, including line items, current status, shipping carrier, tracking number, and estimated delivery date.")]
	public async Task<OrderSummary> GetOrderDetails(
		[Description("The order number, for example 1042.")] int orderId,
		CancellationToken cancellationToken)
	{
		var orders
			 = await orderStore.GetAsync(customer.GetCustomerId(),
				orderId, cancellationToken);

		var orderSummary = new OrderSummary(
			orders.OrderId.ToString(),
			orders.OrderDate,
			orders.Status.ToString(),
			orders.TotalAmount,
			orders.Items.Count,
			orders.Items.Select(x => new ProductSummary(
				x.Product.Name,
				x.Product.Price,
				x.Quantity,
				x.Product.Category.ToString()))
				.ToList());

		return orderSummary;
	}

}