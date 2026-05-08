using FinanceTracker.Application.DTOs;
using MediatR;

namespace FinanceTracker.Application.Budgets.Queries.GetBudgets;

public record GetBudgetsQuery(Guid UserId, int? Month = null, int? Year = null) : IRequest<IReadOnlyList<BudgetDto>>;
