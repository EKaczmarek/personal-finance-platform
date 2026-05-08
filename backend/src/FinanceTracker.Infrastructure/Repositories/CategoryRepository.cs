using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default)
        => await context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId, ct);

    public async Task<IReadOnlyList<Category>> GetByUserAsync(Guid userId, CancellationToken ct = default)
        => await context.Categories.Where(c => c.UserId == userId).OrderBy(c => c.Name).ToListAsync(ct);

    public async Task AddAsync(Category category, CancellationToken ct = default)
        => await context.Categories.AddAsync(category, ct);

    public void Update(Category category)
        => context.Categories.Update(category);

    public void Remove(Category category)
        => context.Categories.Remove(category);
}
