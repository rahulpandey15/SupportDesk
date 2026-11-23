using SupportDesk.Data.Store;
using SupportDesk.Http;
using System.ComponentModel;

namespace SupportDesk.Tools;

public record ShipmentDetails(
		string Carrier, string TrackingNumber, DateTimeOffset ShippedAt,DateTimeOffset EstimatedDelivery,
		List<ShipmentEvents> Events);

public record ShipmentEvents(
		string EventDescription, DateTimeOffset EventTime, string Location);

public sealed class ShipmentTools(IOrderStore orderStore,IShipmentStore shipmentStore, ICustomerContext customer)
{
	[Description("Retrieve shipment tracking details for a specific order that belongs to the signed-in customer. Returns carrier name, tracking number, shipped date, estimated delivery date, and chronological tracking events (status, time, and location). Use this when the user asks about shipping status, tracking progress, or delivery ETA.")]
	public async Task<ShipmentDetails> GetShipmentDetailsAsync(
	[Description("The numeric order ID (for example, 1042) for the signed-in customer's order.")] int orderId,
	CancellationToken cancellationToken)
	{
		//var order = await orderStore.GetAsync(customer.GetCustomerId(), orderId, cancellationToken);
		//if (order is null)
		//	throw new ArgumentException($"Order {orderId} not found for the signed-in customer.");

		//var shipmentDetails = await shipmentStore.GetShipmentDetailsAsync(order, cancellationToken);

		//return shipmentDetails!;

		return new ShipmentDetails("","",DateTimeOffset.Now,DateTimeOffset.Now,new List<ShipmentEvents>());
	}
}
