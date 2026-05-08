using AutoMapper;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Budgets.Commands.CreateBudget;

public class CreateBudgetCommandHandler(
    IBudgetRepository budgetRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateBudgetCommand, BudgetDto>
{
    public async Task<BudgetDto> Handle(CreateBudgetCommand request, CancellationToken ct)
    {
        var budget = Budget.Create(request.UserId, request.CategoryId, request.MonthlyLimit, request.Month, request.Year);
        await budgetRepository.AddAsync(budget, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return mapper.Map<BudgetDto>(budget);
    }
}
