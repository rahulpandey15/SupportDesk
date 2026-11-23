using Microsoft.EntityFrameworkCore;
using SupportDesk.Data.Entities;
using SupportDesk.Tools;

namespace SupportDesk.Data.Store;

public interface IShipmentStore
{
	Task<ShipmentDetails> GetShipmentDetailsAsync(int orderId, CancellationToken ct);
	Task<List<Shipment>> GetShipmentDetailsByTrackingNumberAsync(string trackingNumber, CancellationToken ct);
}

public class ShipmentStore : IShipmentStore
{
	private readonly SupportDeskDbContext dbContext;

	public ShipmentStore(SupportDeskDbContext dbContext)
	{
		this.dbContext = dbContext;
	}


	public async Task<ShipmentDetails> GetShipmentDetailsAsync(
		int orderId,
		CancellationToken ct)
	{
		var orders
			 = await dbContext.Orders.Include(x => x.Shipments).ThenInclude(s => s.Events).FirstOrDefaultAsync(x => x.OrderId == orderId, ct);

		var shipmentDetails = orders.Shipments.Select(s => new ShipmentDetails(
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
