using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default)
        => await context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token, ct);

    public async Task AddAsync(RefreshToken token, CancellationToken ct = default)
        => await context.RefreshTokens.AddAsync(token, ct);

    public void Update(RefreshToken token)
        => context.RefreshTokens.Update(token);
}
