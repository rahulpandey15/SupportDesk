namespace SupportDesk.Data.Entities;

public class AgentSession
{
    public string SessionKey { get; set; } = null!;
    public int CustomerId { get; set; }
    public string AgentName { get; set; } = null!;
    public string ConversationId { get; set; } = null!;
    public string StateJson { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    // Navigation property
    public Customer Customer { get; set; } = null!;
}