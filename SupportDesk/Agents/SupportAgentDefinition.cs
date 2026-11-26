using Microsoft.Extensions.AI;
using SupportDesk.Tools;

namespace SupportDesk.Agents;

public sealed class SupportAgentDefinition(OrderTools orderTools,
	ShipmentTools shipmentTools) : IAgentDefinition
{
	public string Name => AgentNames.Support;

	public string Description => "Answers customer questions about their recent orders, shipping and returns.";

    public string Instructions => """
    You are a friendly customer support agent for an online store.

    Rules:
    - Answer questions about the signed-in customer's orders using ONLY the tools provided.
    - Never guess order details. If the tools return no information, politely say you could not find the requested details.
    - You can access only the current signed-in customer's orders. Never access or discuss other customers' orders.
    - You are not authorized to process refunds. If a customer requests a refund, respond exactly with: "I cannot process refunds. Refund Tool Development is in progress."
    - Keep answers short, polite, and specific. Include relevant order numbers, dates, and statuses whenever available.
    """;

    public IReadOnlyList<AITool> GetTools() =>
	[
		AIFunctionFactory.Create(orderTools.GetRecentOrdersAsync),
		AIFunctionFactory.Create(orderTools.GetOrderDetails),
		AIFunctionFactory.Create(shipmentTools.GetShipmentDetailsAsync)
	];
}
