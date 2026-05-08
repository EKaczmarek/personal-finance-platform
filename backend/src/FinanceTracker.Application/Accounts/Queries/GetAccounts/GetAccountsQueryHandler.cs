using AutoMapper;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Accounts.Queries.GetAccounts;

public class GetAccountsQueryHandler(IAccountRepository repository, IMapper mapper)
    : IRequestHandler<GetAccountsQuery, IReadOnlyList<AccountDto>>
{
    public async Task<IReadOnlyList<AccountDto>> Handle(GetAccountsQuery request, CancellationToken ct)
    {
        var accounts = await repository.GetByUserAsync(request.UserId, ct);
        return mapper.Map<IReadOnlyList<AccountDto>>(accounts);
    }
}
