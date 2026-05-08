using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Interfaces;

public interface IBudgetRepository
{
    Task<Budget?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task<IReadOnlyList<Budget>> GetByUserAsync(Guid userId, int? month = null, int? year = null, CancellationToken ct = default);
    Task AddAsync(Budget budget, CancellationToken ct = default);
    void Update(Budget budget);
    void Remove(Budget budget);
}
