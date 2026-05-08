namespace FinanceTracker.Application.DTOs;

public record BudgetDto(
    Guid Id,
    Guid CategoryId,
    string CategoryName,
    string CategoryColor,
    decimal MonthlyLimit,
    int Month,
    int Year);
