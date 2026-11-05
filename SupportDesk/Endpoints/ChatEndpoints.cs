using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using SupportDesk.Models;
using SupportDesk.Tools;

namespace SupportDesk.Endpoints
{
	public sealed record ChatRequest(string message, string? conversationId = null);
	public static class ChatEndpoints
	{

		public static IEndpointRouteBuilder MapChatEndpoints(
			this IEndpointRouteBuilder app)
		{
			var group = app.MapGroup("/api/agents");

			group.MapPost("/chat", async Task<IResult> (
				ChatRequest request,
				HttpContext httpContext) =>
			{
				var orderTools = httpContext.RequestServices.GetRequiredService<OrderTools>();

				if (string.IsNullOrWhiteSpace(request.message))
				{
					return Results.BadRequest(new
					{
						error = "Message is required"
					});
				}

                var endpoint = Environment.GetEnvironmentVariable("FOUNDRY_ENDPOINT");
				var deploymentName = "gpt-5.4-mini";

                var agentInstructions = """
                      You are a friendly customer support agent for an online store.

                      Rules:
                      - Answer questions about the signed-in customer's orders using ONLY the tools provided.
                      - Never guess order details. If the tools return nothing, say you could not find it.
                      - You can only see the current customer's orders. Never discuss other customers.
                      - You cannot issue refunds. If asked, explain the policy briefly and offer to hand off to a human.
                      - Keep answers short, polite and specific (order numbers, dates, statuses).
                      """;

                AIAgent agent = new AIProjectClient(
								new Uri(endpoint),
								new AzureCliCredential())
					.AsAIAgent(
						model: deploymentName,
						instructions: agentInstructions,

                        tools: [
							  AIFunctionFactory.Create(orderTools.GetOrderDetails),
                              AIFunctionFactory.Create(orderTools.GetRecentOrdersAsync)
                            ],
						name: "HelloAgent");



				var repsonse = await agent.RunAsync<List<OrderAgentResponse>>(request.message);


				return Results.Ok(repsonse.Result);
			});

			return app;
		}

	}
}
