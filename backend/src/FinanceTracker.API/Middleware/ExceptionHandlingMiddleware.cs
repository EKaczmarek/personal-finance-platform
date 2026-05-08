using System.Net;
using System.Text.Json;

namespace FinanceTracker.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (KeyNotFoundException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.NotFound, "Not Found", ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.Unauthorized, "Unauthorized", ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteErrorAsync(context, HttpStatusCode.InternalServerError, "Internal Server Error",
                "An unexpected error occurred.");
        }
    }

    private static Task WriteErrorAsync(HttpContext context, HttpStatusCode status, string title, string detail)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/problem+json";

        var problem = new
        {
            type = $"https://httpstatuses.com/{(int)status}",
            title,
            status = (int)status,
            detail
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
