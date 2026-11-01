namespace SupportDesk.Data.Entities;

public class ShipmentEvent
{
    public int EventId { get; set; }
    public int ShipmentId { get; set; }
    public DateTime EventTime { get; set; }
    public string? Location { get; set; }
    public string Description { get; set; } = string.Empty;

    public Shipment Shipment { get; set; } = null!;
    
}