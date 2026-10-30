using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.Agents.AI;

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
                ChatRequest request) =>
            {
                if (string.IsNullOrWhiteSpace(request.message))
                {
                    return Results.BadRequest(new
                    {
                        error = "Message is required"
                    });
                }

                var endpoint = "";
                var deploymentName = "";

                AIAgent agent = new AIProjectClient(
                                new Uri(endpoint), 
                                new AzureCliCredential())
                    .AsAIAgent(
                        model: deploymentName,
                        instructions: "You are a friendly assistant. Keep your answers brief.",
                        name: "HelloAgent");



                var repsonse = await agent.RunAsync(request.message);


                return Results.Ok(repsonse.Text);
            });

            return app;
        }

    }
}
