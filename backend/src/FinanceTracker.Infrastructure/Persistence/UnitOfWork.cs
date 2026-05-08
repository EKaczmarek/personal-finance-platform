using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Infrastructure.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);
}
