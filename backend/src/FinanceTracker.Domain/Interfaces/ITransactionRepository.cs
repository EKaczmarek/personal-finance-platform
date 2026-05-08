using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task<IReadOnlyList<Transaction>> GetByUserAsync(Guid userId, int? month = null, int? year = null, CancellationToken ct = default);
    Task AddAsync(Transaction transaction, CancellationToken ct = default);
    void Update(Transaction transaction);
    void Remove(Transaction transaction);
}
