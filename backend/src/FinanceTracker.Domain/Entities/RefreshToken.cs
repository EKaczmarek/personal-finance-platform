using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;

    private RefreshToken() { }

    public static RefreshToken Create(Guid userId, string token, DateTime expiresAt)
    {
        return new RefreshToken
        {
            UserId = userId,
            Token = token,
            ExpiresAt = expiresAt,
            IsRevoked = false
        };
    }

    public bool IsValid() => !IsRevoked && ExpiresAt > DateTime.UtcNow;

    public void Revoke()
    {
        IsRevoked = true;
        SetUpdatedAt();
    }
}
