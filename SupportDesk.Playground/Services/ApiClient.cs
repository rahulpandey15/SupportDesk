 using System.Net;
using System.Net.Http.Json;

namespace SupportDesk.Playground.Services;

public sealed class ApiClient(HttpClient httpClient)
{
	public async Task<ChatResult<string>> SendPromptAsync(string message,int customerId, string? conversationId, CancellationToken cancellationToken)
	{
		var request = new ChatRequest(message, conversationId);
		using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/agents/support/chat")
		{
			Content = JsonContent.Create(request)
		};
		httpRequest.Headers.Add("x-customer-id", customerId.ToString());

		using var response = await httpClient.SendAsync(httpRequest, cancellationToken);

		if (response.IsSuccessStatusCode)
		{
			var result = await response.Content.ReadFromJsonAsync<ChatResult<string>>(cancellationToken: cancellationToken);
			if (result is null)
			{
				throw new InvalidOperationException("The API returned an empty response.");
			}

			return result;
		}

		var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
		throw new ApiClientException(response.StatusCode, errorBody);
	}
}

public sealed class ApiClientException(HttpStatusCode statusCode, string? message)
	: Exception($"API request failed with status {(int)statusCode} ({statusCode}). {message}".Trim())
{
	public HttpStatusCode StatusCode { get; } = statusCode;
}

public sealed record ChatRequest(string message, string? conversationId = null);
public sealed record ChatResult<T>(T data, string conversationId);
