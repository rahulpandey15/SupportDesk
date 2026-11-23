using SupportDesk.Data.Entities;
using SupportDesk.Tools;

namespace SupportDesk.Data.Store;

public interface IShipmentStore
{
	Task<ShipmentDetails> GetShipmentDetailsAsync(Order? order, CancellationToken ct);
	Task<List<Shipment>> GetShipmentDetailsByTrackingNumberAsync(string trackingNumber, CancellationToken ct);
}

public class ShipmentStore : IShipmentStore
{
	public async Task<ShipmentDetails> GetShipmentDetailsAsync(
		Order? order, 
		CancellationToken ct)
	{
		var shipmentDetails = order.Shipments.Select(s => new ShipmentDetails(
			s.Carrier,
			s.TrackingNumber,
			s.ShippedAt.Value,
			s.EstimatedDelivery.Value,
			s.Events.Select(e => new ShipmentEvents(
				e.Description,
				e.EventTime,
				e.Location)).ToList()
		)).FirstOrDefault();

		return await Task.FromResult(shipmentDetails);

	}

	public Task<List<Shipment>> GetShipmentDetailsByTrackingNumberAsync(
		string trackingNumber, 
		CancellationToken ct)
	{
		throw new NotImplementedException();
	}
}
