

using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Postex.SharedKernel.Middlewares;

public class CorrelationldLogMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationldLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        string? correlationId = null;
        // Check if the CorrelationId header is already present
        if (!context.Request.Headers.ContainsKey("x-correlation-id"))
        {
            // Generate a unique identifier
             correlationId = Guid.NewGuid().ToString();

            // Set the CorrelationId header
            context.Request.Headers.Add("x-correlation-id", correlationId);
        }
        else
        {
            correlationId = context.Request.Headers["x-correlation-id"];
        }
        // Enrich the log context with CorrelationId
        using (LogContext.PushProperty("CorrelationId", correlationId)) 
        {
            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}