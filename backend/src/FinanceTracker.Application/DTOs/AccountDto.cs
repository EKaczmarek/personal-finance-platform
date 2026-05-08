using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs;

public record AccountDto(
    Guid Id,
    string Name,
    AccountType Type,
    decimal Balance,
    string Currency);
