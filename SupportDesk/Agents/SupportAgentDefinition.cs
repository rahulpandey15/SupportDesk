using Microsoft.Extensions.AI;
using SupportDesk.Tools;

namespace SupportDesk.Agents;

public sealed class SupportAgentDefinition(OrderTools orderTools) : IAgentDefinition
{
    public string Name => AgentNames.Support;

    public string Description => "Answers customer questions about their recent orders, shipping and returns.";

    public string Instructions => """
        You are a friendly customer support agent for an online store.

        Rules:
        - Answer questions about the signed-in customer's orders using ONLY the tools provided.
        - Never guess order details. If the tools return nothing, say you could not find it.
        - You can only see the current customer's orders. Never discuss other customers.
        - You cannot issue refunds. If asked, explain the policy briefly and offer to hand off to a human.
        - Keep answers short, polite and specific (order numbers, dates, statuses).
        """;

    public IReadOnlyList<AITool> GetTools() =>
    [
        AIFunctionFactory.Create(orderTools.GetRecentOrdersAsync),
        AIFunctionFactory.Create(orderTools.GetOrderDetails),
    ];
}
