using System.ComponentModel;
using SupportDesk.Data.Entities;
using SupportDesk.Data.Store;
using SupportDesk.Http;

namespace SupportDesk.Tools;

public sealed record OrderSummary(string OrderId, DateTimeOffset PlacedAt, string Status, decimal Total, int ItemCount);



public sealed class OrderTools(IOrderStore orderStore, ICustomerContext customer)
{
    
    [Description("Get the signed in customer's most recent orders, newest first")]
    public async Task<IReadOnlyList<OrderSummary>> GetRecentOrdersAsync(
        [Description("How many orders to return, between 1 and 10.")] int count, 
        CancellationToken ct)
    {
        var orders = await orderStore.GetRecentOrderByCustomerAsync(customer.GetCustomerId(), count, ct);

        return orders.Select(x => new OrderSummary(
            x.OrderId.ToString(),
            x.OrderDate,
            x.Status.ToString(),
            x.TotalAmount,
            x.Items.Count)).ToList();
    }
    
    
    [Description("Gets full details for one of the signed-in customer's orders: items, status, carrier, tracking number and estimated delivery.")]
    public async Task<Order?>  GetOrderDetails(
        [Description("The order number, for example 1042.")] int orderId,
        CancellationToken cancellationToken) =>
        await orderStore.GetAsync(customer.GetCustomerId(), orderId, cancellationToken);
    
}