using System.Security.Claims;
using FinanceTracker.API.Extensions;
using FinanceTracker.Application.Transactions.Commands.CreateTransaction;
using FinanceTracker.Application.Transactions.Commands.DeleteTransaction;
using FinanceTracker.Application.Transactions.Queries.GetTransactions;
using MediatR;

namespace FinanceTracker.API.Endpoints;

public static class TransactionEndpoints
{
    public static IEndpointRouteBuilder MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/transactions")
            .WithTags("Transactions")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, ClaimsPrincipal user, int? month, int? year) =>
        {
            var result = await mediator.Send(new GetTransactionsQuery(user.GetUserId(), month, year));
            return Results.Ok(result);
        });

        group.MapPost("/", async (IMediator mediator, ClaimsPrincipal user, CreateTransactionCommand command) =>
        {
            var cmd = command with { UserId = user.GetUserId() };
            var result = await mediator.Send(cmd);
            return Results.Created($"/api/v1/transactions/{result.Id}", result);
        });

        group.MapDelete("/{id:guid}", async (IMediator mediator, ClaimsPrincipal user, Guid id) =>
        {
            await mediator.Send(new DeleteTransactionCommand(id, user.GetUserId()));
            return Results.NoContent();
        });

        return app;
    }
}
