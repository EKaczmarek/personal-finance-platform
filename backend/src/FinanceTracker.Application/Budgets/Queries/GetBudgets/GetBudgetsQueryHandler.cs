using AutoMapper;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Budgets.Queries.GetBudgets;

public class GetBudgetsQueryHandler(IBudgetRepository repository, IMapper mapper)
    : IRequestHandler<GetBudgetsQuery, IReadOnlyList<BudgetDto>>
{
    public async Task<IReadOnlyList<BudgetDto>> Handle(GetBudgetsQuery request, CancellationToken ct)
    {
        var budgets = await repository.GetByUserAsync(request.UserId, request.Month, request.Year, ct);
        return mapper.Map<IReadOnlyList<BudgetDto>>(budgets);
    }
}
