using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

public class AccountRepository(AppDbContext context) : IAccountRepository
{
    public async Task<Account?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default)
        => await context.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId, ct);

    public async Task<IReadOnlyList<Account>> GetByUserAsync(Guid userId, CancellationToken ct = default)
        => await context.Accounts.Where(a => a.UserId == userId).OrderBy(a => a.Name).ToListAsync(ct);

    public async Task AddAsync(Account account, CancellationToken ct = default)
        => await context.Accounts.AddAsync(account, ct);

    public void Update(Account account)
        => context.Accounts.Update(account);

    public void Remove(Account account)
        => context.Accounts.Remove(account);
}
