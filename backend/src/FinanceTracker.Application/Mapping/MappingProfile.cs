using AutoMapper;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Transaction, TransactionDto>()
            .ConstructUsing(t => new TransactionDto(
                t.Id, t.Amount, t.Description, t.Type, t.OccurredAt,
                t.AccountId, t.Account != null ? t.Account.Name : string.Empty,
                t.CategoryId, t.Category != null ? t.Category.Name : string.Empty,
                t.Category != null ? t.Category.Color : string.Empty,
                t.CreatedAt));

        CreateMap<Account, AccountDto>()
            .ConstructUsing(a => new AccountDto(a.Id, a.Name, a.Type, a.Balance, a.Currency));

        CreateMap<Category, CategoryDto>()
            .ConstructUsing(c => new CategoryDto(c.Id, c.Name, c.Icon, c.Color));

        CreateMap<Budget, BudgetDto>()
            .ConstructUsing(b => new BudgetDto(
                b.Id, b.CategoryId,
                b.Category != null ? b.Category.Name : string.Empty,
                b.Category != null ? b.Category.Color : string.Empty,
                b.MonthlyLimit, b.Month, b.Year));
    }
}
