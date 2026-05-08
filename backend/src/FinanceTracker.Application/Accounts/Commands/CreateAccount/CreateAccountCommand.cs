using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Accounts.Commands.CreateAccount;

public record CreateAccountCommand(
    Guid UserId,
    string Name,
    AccountType Type,
    decimal InitialBalance,
    string Currency = "PLN") : IRequest<AccountDto>;
