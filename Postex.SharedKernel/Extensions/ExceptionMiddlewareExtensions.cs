using Microsoft.AspNetCore.Builder;
using Postex.SharedKernel.Middlewares;

namespace Postex.SharedKernel.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
