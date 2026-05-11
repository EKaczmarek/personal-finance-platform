using System.Security.Claims;
using FinanceTracker.API.Extensions;
using FinanceTracker.Application.Accounts.Commands.CreateAccount;
using FinanceTracker.Application.Accounts.Queries.GetAccounts;
using MediatR;

namespace FinanceTracker.API.Endpoints;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/accounts")
            .WithTags("Accounts")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, ClaimsPrincipal user) =>
        {
            var result = await mediator.Send(new GetAccountsQuery(user.GetUserId()));
            return Results.Ok(result);
        });

        group.MapPost("/", async (IMediator mediator, ClaimsPrincipal user, CreateAccountCommand command) =>
        {
            var cmd = command with { UserId = user.GetUserId() };
            var result = await mediator.Send(cmd);
            return Results.Created($"/api/v1/accounts/{result.Id}", result);
        });

        return app;
    }
}
