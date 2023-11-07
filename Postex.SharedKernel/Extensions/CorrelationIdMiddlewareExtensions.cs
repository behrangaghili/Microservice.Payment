using Microsoft.AspNetCore.Builder;
using Postex.SharedKernel.Middlewares;

namespace Postex.SharedKernel.Extensions;

public static class CorrelationIdMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorrelationldLogMiddleware>();
    }
}
