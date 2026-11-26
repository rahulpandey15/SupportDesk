using System.Reflection;

namespace SupportDesk.Agents;

public static class AgentServiceCollectionExtensions
{
    /// <summary>
    /// Scans an assembly and registers every IAgentDefinition (scoped), so a new agent
    /// is just a new class. Definitions are scoped so they can inject scoped tool classes.
    /// </summary>
    public static IServiceCollection AddAgentDefinitions(this IServiceCollection services, Assembly assembly)
    {
        var definitionTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(IAgentDefinition).IsAssignableFrom(t));

        foreach (var type in definitionTypes)
        {
            services.AddScoped(typeof(IAgentDefinition), type);
        }

        return services;
    }
}
