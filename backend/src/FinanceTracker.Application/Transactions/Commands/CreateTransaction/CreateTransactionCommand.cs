using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(
    Guid UserId,
    Guid AccountId,
    Guid CategoryId,
    decimal Amount,
    TransactionType Type,
    string Description,
    DateTime? OccurredAt) : IRequest<TransactionDto>;
