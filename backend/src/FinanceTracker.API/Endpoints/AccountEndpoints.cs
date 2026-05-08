using FinanceTracker.Application.Accounts.Commands.CreateAccount;
using FinanceTracker.Application.Accounts.Queries.GetAccounts;
using MediatR;

namespace FinanceTracker.API.Endpoints;

public static class AccountEndpoints
{
    private static readonly Guid DevUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/accounts").WithTags("Accounts");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAccountsQuery(DevUserId));
            return Results.Ok(result);
        });

        group.MapPost("/", async (IMediator mediator, CreateAccountCommand command) =>
        {
            var cmd = command with { UserId = DevUserId };
            var result = await mediator.Send(cmd);
            return Results.Created($"/api/v1/accounts/{result.Id}", result);
        });

        return app;
    }
}
