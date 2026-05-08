using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task<IReadOnlyList<Account>> GetByUserAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Account account, CancellationToken ct = default);
    void Update(Account account);
    void Remove(Account account);
}
