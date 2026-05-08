using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Transaction : BaseEntity
{
    public decimal Amount { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public TransactionType Type { get; private set; }
    public DateTime OccurredAt { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public Guid UserId { get; private set; }

    public Account Account { get; private set; } = null!;
    public Category Category { get; private set; } = null!;
    public User User { get; private set; } = null!;

    private Transaction() { }

    public static Transaction Create(
        Guid userId,
        Guid accountId,
        Guid categoryId,
        decimal amount,
        TransactionType type,
        string description,
        DateTime? occurredAt = null)
    {
        return new Transaction
        {
            UserId = userId,
            AccountId = accountId,
            CategoryId = categoryId,
            Amount = amount,
            Type = type,
            Description = description,
            OccurredAt = occurredAt ?? DateTime.UtcNow
        };
    }

    public void Update(decimal amount, string description, Guid categoryId, DateTime occurredAt)
    {
        Amount = amount;
        Description = description;
        CategoryId = categoryId;
        OccurredAt = occurredAt;
        SetUpdatedAt();
    }
}
