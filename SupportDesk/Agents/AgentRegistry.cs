using Microsoft.Agents.AI;

namespace SupportDesk.Agents;

public sealed record AgentInfo(string Name, string Description);

public interface IAgentRegistry
{
    IReadOnlyList<AgentInfo> List();
    bool TryGet(string name, out AIAgent agent);
}

public sealed class AgentRegistry : IAgentRegistry
{
    private readonly Dictionary<string, IAgentDefinition> _definitions;
    private readonly Dictionary<string, AIAgent> _built = new(StringComparer.OrdinalIgnoreCase);
    private readonly IAgentBuilder _builder;

    public AgentRegistry(IEnumerable<IAgentDefinition> definitions, IAgentBuilder builder)
    {
        _builder = builder;
        _definitions = definitions.ToDictionary(d => d.Name, StringComparer.OrdinalIgnoreCase);
    }

    public IReadOnlyList<AgentInfo> List() =>
        _definitions.Values.Select(d => new AgentInfo(d.Name, d.Description)).ToList();

    public bool TryGet(string name, out AIAgent agent)
    {
        if (_built.TryGetValue(name, out agent!))
        {
            return true;
        }

        if (!_definitions.TryGetValue(name, out var definition))
        {
            agent = null!;
            return false;
        }

        agent = _built[name] = _builder.Build(definition);
        return true;
    }
}
