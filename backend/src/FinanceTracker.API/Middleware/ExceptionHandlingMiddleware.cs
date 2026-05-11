using System.Net;
using System.Text.Json;
using FluentValidation;

namespace FinanceTracker.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            await WriteErrorAsync(context, HttpStatusCode.BadRequest, "Validation Failed",
                JsonSerializer.Serialize(errors));
        }
        catch (InvalidOperationException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.Conflict, "Conflict", ex.Message);
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
