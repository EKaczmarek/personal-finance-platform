using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default);
    Task AddAsync(RefreshToken token, CancellationToken ct = default);
    void Update(RefreshToken token);
}
