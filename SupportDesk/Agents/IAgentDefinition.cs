using Microsoft.Extensions.AI;

namespace SupportDesk.Agents;


public interface IAgentDefinition
{
    string Name { get; }
    string Description { get; }
    string Instructions { get; }
    IReadOnlyList<AITool> GetTools();
}
