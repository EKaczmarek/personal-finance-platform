using FinanceTracker.Application.Transactions.Commands.CreateTransaction;
using FinanceTracker.Application.Transactions.Commands.DeleteTransaction;
using FinanceTracker.Application.Transactions.Queries.GetTransactions;
using MediatR;

namespace FinanceTracker.API.Endpoints;

public static class TransactionEndpoints
{
    // Temporary dev user until auth is wired (Day 6)
    private static readonly Guid DevUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public static IEndpointRouteBuilder MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/transactions").WithTags("Transactions");

        group.MapGet("/", async (IMediator mediator, int? month, int? year) =>
        {
            var result = await mediator.Send(new GetTransactionsQuery(DevUserId, month, year));
            return Results.Ok(result);
        });

        group.MapPost("/", async (IMediator mediator, CreateTransactionCommand command) =>
        {
            var cmd = command with { UserId = DevUserId };
            var result = await mediator.Send(cmd);
            return Results.Created($"/api/v1/transactions/{result.Id}", result);
        });

        group.MapDelete("/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            await mediator.Send(new DeleteTransactionCommand(id, DevUserId));
            return Results.NoContent();
        });

        return app;
    }
}
