using FinanceTracker.Application.DTOs;
using MediatR;

namespace FinanceTracker.Application.Budgets.Commands.CreateBudget;

public record CreateBudgetCommand(
    Guid UserId,
    Guid CategoryId,
    decimal MonthlyLimit,
    int Month,
    int Year) : IRequest<BudgetDto>;
