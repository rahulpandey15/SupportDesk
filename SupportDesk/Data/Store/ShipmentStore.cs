using SupportDesk.Data.Entities;

namespace SupportDesk.Data.Store;

public interface IShipmentStore
{
	Task<List<Order>> GetShipmentDetailsByOrderIdAsync(string orderId, CancellationToken ct);
	Task<List<Order>> GetShipmentDetailsByTrackingNumberAsync(string trackingNumber, CancellationToken ct);
}

public class ShipmentStore : IShipmentStore
{
	public Task<List<Order>> GetShipmentDetailsByOrderIdAsync(
		string orderId, 
		CancellationToken ct)
	{
		throw new NotImplementedException();
	}

	public Task<List<Order>> GetShipmentDetailsByTrackingNumberAsync(
		string trackingNumber, 
		CancellationToken ct)
	{
		throw new NotImplementedException();
	}
}
