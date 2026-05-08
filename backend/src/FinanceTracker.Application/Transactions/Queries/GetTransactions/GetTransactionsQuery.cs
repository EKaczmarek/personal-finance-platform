using FinanceTracker.Application.DTOs;
using MediatR;

namespace FinanceTracker.Application.Transactions.Queries.GetTransactions;

public record GetTransactionsQuery(Guid UserId, int? Month, int? Year) : IRequest<IReadOnlyList<TransactionDto>>;
