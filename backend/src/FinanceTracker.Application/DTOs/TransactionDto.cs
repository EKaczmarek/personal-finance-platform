using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs;

public record TransactionDto(
    Guid Id,
    decimal Amount,
    string Description,
    TransactionType Type,
    DateTime OccurredAt,
    Guid AccountId,
    string AccountName,
    Guid CategoryId,
    string CategoryName,
    string CategoryColor,
    DateTime CreatedAt);
