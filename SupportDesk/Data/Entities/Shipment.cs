namespace SupportDesk.Data.Entities;

public class Shipment
{
    public int ShipmentId { get; set; }
    public int OrderId { get; set; }
    public string Carrier { get; set; } = string.Empty;
    public string TrackingNumber { get; set; } = string.Empty;
    public DateTime? ShippedAt { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public ShipmentStatus Status { get; set; }

    public Order Order { get; set; } = null!;
    public ICollection<ShipmentEvent> Events { get; set; } = [];
}