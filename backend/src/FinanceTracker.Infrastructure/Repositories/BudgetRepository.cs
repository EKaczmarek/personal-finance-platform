using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

public class BudgetRepository(AppDbContext context) : IBudgetRepository
{
    public async Task<Budget?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default)
        => await context.Budgets
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId, ct);

    public async Task<IReadOnlyList<Budget>> GetByUserAsync(Guid userId, int? month, int? year, CancellationToken ct = default)
    {
        var query = context.Budgets.Include(b => b.Category).Where(b => b.UserId == userId);
        if (month.HasValue) query = query.Where(b => b.Month == month.Value);
        if (year.HasValue)  query = query.Where(b => b.Year == year.Value);
        return await query.ToListAsync(ct);
    }

    public async Task AddAsync(Budget budget, CancellationToken ct = default)
        => await context.Budgets.AddAsync(budget, ct);

    public void Update(Budget budget)
        => context.Budgets.Update(budget);

    public void Remove(Budget budget)
        => context.Budgets.Remove(budget);
}
