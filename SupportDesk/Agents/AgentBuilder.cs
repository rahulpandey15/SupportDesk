using Azure.AI.Projects;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using SupportDesk.Configuration;

namespace SupportDesk.Agents;


public interface IAgentBuilder
{
    AIAgent Build(IAgentDefinition definition);
}

public sealed class FoundryAgentBuilder(
    AIProjectClient projectClient,
    IOptions<FoundryOptions> options) : IAgentBuilder
{
    public AIAgent Build(IAgentDefinition definition) =>
        projectClient.AsAIAgent(
            model: options.Value.ModelDeployment,
            instructions: definition.Instructions,
            name: definition.Name,
            tools: definition.GetTools().ToList());
}
