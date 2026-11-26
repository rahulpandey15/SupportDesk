using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using SupportDesk.Agents;
using SupportDesk.Http;
using SupportDesk.Sessions;

namespace SupportDesk.Services
{
	public sealed record ChatResult(string AgentName, string ConversationId, string Reply);

	public sealed class AgentChatService(
	 IAgentRegistry registry,
	 ISessionStore sessions,
	 ICustomerContext customer,
	 ILogger<AgentChatService> logger)
	{
		public async Task<ChatResult?> ChatAsync(
			string agentName,
			string? conversationId,
			string message,
			CancellationToken ct)
		{
			if (!registry.TryGet(agentName, out var agent))
			{
				return null;
			}

			conversationId ??= Guid.NewGuid().ToString("N");

			var key = new SessionKey(customer.GetCustomerId(), conversationId);

			var saved = await sessions.GetAsync(key, ct);
			AgentSession session = saved is { } state
				? await agent.DeserializeSessionAsync(state, cancellationToken: ct)
				: await agent.CreateSessionAsync(ct);

			logger.LogInformation("Agent {Agent} handling turn for conversation {Conversation}", agentName, conversationId);

			var response = await agent.RunAsync(message, session, cancellationToken: ct);

			await sessions.SaveAsync(
				key, await agent.SerializeSessionAsync(session, cancellationToken: ct), ct);

			if (IsApprovalToolInvoked(response))
			{
				var toolApprovalRequests = response.Messages
										.SelectMany(m => m.Contents)
										.OfType<ToolApprovalRequestContent>()
										.ToList();

				var functionCall = (FunctionCallContent)toolApprovalRequests.FirstOrDefault().ToolCall;

				string responseMessage
					 = $"This request cannot be process as tool {functionCall.Name} requires an approval.";

				return new ChatResult(agentName, conversationId, responseMessage);
			}
			return new ChatResult(agentName, conversationId, response.Text);
		}



		private bool IsApprovalToolInvoked(AgentResponse response) =>
								response.Messages
										.SelectMany(m => m.Contents)
										.OfType<ToolApprovalRequestContent>()
										.Any();
	}
}
