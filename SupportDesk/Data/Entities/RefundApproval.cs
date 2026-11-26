namespace SupportDesk.Data.Entities;

public class RefundApproval
{
    public int ApprovalId { get; set; }
    public int RefundId { get; set; }
    public ApprovalStatus Status { get; set; }
    public DateTime RequestedAt { get; set; }
    public string? DecidedBy { get; set; }
    public DateTime? DecidedAt { get; set; }
    public string? Comment { get; set; }

    public Refund Refund { get; set; } = null!;
}
