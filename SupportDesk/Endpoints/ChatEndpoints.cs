using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using SupportDesk.Http;
using SupportDesk.Models;
using SupportDesk.Sessions;
using SupportDesk.Tools;

namespace SupportDesk.Endpoints
{
	public sealed record ChatRequest(string message, string? conversationId = null);
	public sealed record ChatResult<T>(T data, string conversationId);
	
	public static class ChatEndpoints
	{

		public static IEndpointRouteBuilder MapChatEndpoints(
			this IEndpointRouteBuilder app)
		{
			var group = app.MapGroup("/api/agents");

			group.MapPost("/chat", async Task<ChatResult<List<OrderAgentResponse>>> (
				ChatRequest request,
				HttpContext httpContext,
				ICustomerContext customer,
				ISessionStore sessionStore,
				CancellationToken ct
				) =>
			{
				var orderTools = httpContext.RequestServices.GetRequiredService<OrderTools>();
				
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


				if (!string.IsNullOrEmpty(request.conversationId))
				{
					var key = new SessionKey(customer.GetCustomerId(), request.conversationId);
					var saved = await sessionStore.GetAsync(key, ct);
					AgentSession agentSession = saved is { } state
							? await agent.DeserializeSessionAsync(state, cancellationToken: ct)
							: await agent.CreateSessionAsync(ct);

					var response = await agent.RunAsync<List<OrderAgentResponse>>(request.message,agentSession,cancellationToken:ct);
					await sessionStore.SaveAsync(key, await agent.SerializeSessionAsync(agentSession, cancellationToken: ct), ct);
					return new ChatResult<List<OrderAgentResponse>>(response.Result, request.conversationId!);
				}
				else
				{
					var conversationId = Guid.NewGuid().ToString("N");
				    var response = await agent.RunAsync<List<OrderAgentResponse>>(request.message);
					return new ChatResult<List<OrderAgentResponse>>(response.Result, conversationId);
				}
			});

			return app;
		}

	}
}
