namespace SupportDesk.Http;

public class CustomerContextMiddleware(RequestDelegate next)
{

    public const string HeaderName = "x-customer-id";

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            if (!context.Request.Headers.TryGetValue(HeaderName, out var value) ||
                string.IsNullOrWhiteSpace(value))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = $"Missing {HeaderName} header." });
                return;
            }

           context.Request.Headers.Add("CustomerId", value);
        }

        await next(context);
    }

}