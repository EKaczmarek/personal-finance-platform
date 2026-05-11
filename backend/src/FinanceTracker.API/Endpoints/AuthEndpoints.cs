using FinanceTracker.Application.Auth.Commands.Login;
using FinanceTracker.Application.Auth.Commands.Logout;
using FinanceTracker.Application.Auth.Commands.RefreshToken;
using FinanceTracker.Application.Auth.Commands.Register;
using MediatR;

namespace FinanceTracker.API.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/auth").WithTags("Auth");

        group.MapPost("/register", async (IMediator mediator, RegisterCommand command) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        });

        group.MapPost("/login", async (IMediator mediator, LoginCommand command) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        });

        group.MapPost("/refresh", async (IMediator mediator, RefreshTokenCommand command) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        });

        group.MapPost("/logout", async (IMediator mediator, LogoutCommand command) =>
        {
            await mediator.Send(command);
            return Results.NoContent();
        });

        return app;
    }
}
