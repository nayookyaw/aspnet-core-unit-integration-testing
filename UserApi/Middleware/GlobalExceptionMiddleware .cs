
using System.Text.Json;
using UserApi.Utils;

namespace UserApi.Middleware;

public sealed class GlobalExceptionMiddleware : IMiddleware
{
    // private readonly ILogger<GlobalExceptionMiddleware> _logger;

    // public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
    // {
    //     _logger = logger;
    // }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // _logger.LogError(ex, "An unhandled exception occurred.");
            context.Response.StatusCode = 500;
            var payload = new
            {
                Message = "An unexpected error occurred. Please try again later.",
                StatusCode = 500
            };
            SerilogUtil.LogError(ex.Message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}