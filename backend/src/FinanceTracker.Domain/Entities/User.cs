using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    private readonly List<Account> _accounts = [];
    private readonly List<RefreshToken> _refreshTokens = [];

    public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private User() { }

    public static User Create(string email, string passwordHash, string firstName, string lastName)
    {
        return new User
        {
            Email = email.ToLowerInvariant(),
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName
        };
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        SetUpdatedAt();
    }
}
