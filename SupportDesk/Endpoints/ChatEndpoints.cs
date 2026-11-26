using SupportDesk.Agents;
using SupportDesk.Services;

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


			group.MapGet("/", (IAgentRegistry registry) => Results.Ok(registry.List()));

			group.MapPost("/{agentName}/chat", async Task<IResult> (
		   string agentName,
		   ChatRequest request,
		   AgentChatService chat,
		   CancellationToken ct) =>
			{
				if (string.IsNullOrWhiteSpace(
					request.message))
				{
					return Results.BadRequest(new { error = "Message is required." });
				}

				var result = await chat.ChatAsync(agentName, request.conversationId, request.message, ct);

				return result is null
					? Results.NotFound(new { error = $"Unknown agent '{agentName}'." })
					: Results.Ok(new ChatResult<string>(result.Reply, result.ConversationId));
			});

			return app;
		}

	}
}
