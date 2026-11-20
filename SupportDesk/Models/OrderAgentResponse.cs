namespace SupportDesk.Models;

public class OrderAgentResponse
{
    public string OrderName { get; set; } = default!;
    public decimal TotalAmont {  get; set; }
    public string OrderStatus { get; set; } = default!;
    public string Product { get; set; } = default!;
}
