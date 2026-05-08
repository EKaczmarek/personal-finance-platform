using AutoMapper;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Transactions.Queries.GetTransactions;

public class GetTransactionsQueryHandler(ITransactionRepository repository, IMapper mapper)
    : IRequestHandler<GetTransactionsQuery, IReadOnlyList<TransactionDto>>
{
    public async Task<IReadOnlyList<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken ct)
    {
        var transactions = await repository.GetByUserAsync(request.UserId, request.Month, request.Year, ct);
        return mapper.Map<IReadOnlyList<TransactionDto>>(transactions);
    }
}
