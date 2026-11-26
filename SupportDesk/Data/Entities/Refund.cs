namespace SupportDesk.Data.Entities;

public class Refund
{
    public int RefundId { get; set; }
    public int OrderId { get; set; }
    public int PaymentId { get; set; }
    public int? ReturnId { get; set; }       
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public RefundStatus Status { get; set; }
    public bool RequiresApproval { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }

    public Order Order { get; set; } = null!;
    public Payment Payment { get; set; } = null!;
    public ICollection<RefundApproval> Approvals { get; set; } = [];
}
