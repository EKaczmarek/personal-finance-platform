using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), ct);

    public async Task AddAsync(User user, CancellationToken ct = default)
        => await context.Users.AddAsync(user, ct);

    public void Update(User user)
        => context.Users.Update(user);
}
