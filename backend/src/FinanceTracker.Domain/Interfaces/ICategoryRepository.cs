using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task<IReadOnlyList<Category>> GetByUserAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Category category, CancellationToken ct = default);
    void Update(Category category);
    void Remove(Category category);
}
