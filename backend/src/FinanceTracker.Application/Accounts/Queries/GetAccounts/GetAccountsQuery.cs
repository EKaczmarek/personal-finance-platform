using FinanceTracker.Application.DTOs;
using MediatR;

namespace FinanceTracker.Application.Accounts.Queries.GetAccounts;

public record GetAccountsQuery(Guid UserId) : IRequest<IReadOnlyList<AccountDto>>;
