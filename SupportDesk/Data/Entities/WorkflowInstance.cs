namespace SupportDesk.Data.Entities;

public class WorkflowInstance
{
    public int WorkflowId { get; set; }
    public string Type { get; set; } = string.Empty;        
    public string CorrelationId { get; set; } = string.Empty;  
    public WorkflowState State { get; set; }
    public string StateJson { get; set; } = "{}";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
