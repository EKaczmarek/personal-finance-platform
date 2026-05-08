using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

public class TransactionRepository(AppDbContext context) : ITransactionRepository
{
    public async Task<Transaction?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default)
        => await context.Transactions
            .Include(t => t.Account)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId, ct);

    public async Task<IReadOnlyList<Transaction>> GetByUserAsync(Guid userId, int? month, int? year, CancellationToken ct = default)
    {
        var query = context.Transactions
            .Include(t => t.Account)
            .Include(t => t.Category)
            .Where(t => t.UserId == userId);

        if (month.HasValue && year.HasValue)
            query = query.Where(t => t.OccurredAt.Month == month.Value && t.OccurredAt.Year == year.Value);

        return await query.OrderByDescending(t => t.OccurredAt).ToListAsync(ct);
    }

    public async Task AddAsync(Transaction transaction, CancellationToken ct = default)
        => await context.Transactions.AddAsync(transaction, ct);

    public void Update(Transaction transaction)
        => context.Transactions.Update(transaction);

    public void Remove(Transaction transaction)
        => context.Transactions.Remove(transaction);
}
