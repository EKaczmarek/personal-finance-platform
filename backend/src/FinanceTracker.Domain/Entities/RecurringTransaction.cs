using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class RecurringTransaction : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public RecurringFrequency Frequency { get; private set; }
    public DateTime NextDate { get; private set; }
    public bool IsActive { get; private set; } = true;

    public User User { get; private set; } = null!;
    public Account Account { get; private set; } = null!;
    public Category Category { get; private set; } = null!;

    private RecurringTransaction() { }

    public static RecurringTransaction Create(
        Guid userId, Guid accountId, Guid categoryId,
        decimal amount, TransactionType type,
        string description, RecurringFrequency frequency, DateTime nextDate)
    {
        return new RecurringTransaction
        {
            UserId = userId,
            AccountId = accountId,
            CategoryId = categoryId,
            Amount = amount,
            Type = type,
            Description = description,
            Frequency = frequency,
            NextDate = nextDate
        };
    }

    public void AdvanceNextDate()
    {
        NextDate = Frequency == RecurringFrequency.Weekly
            ? NextDate.AddDays(7)
            : NextDate.AddMonths(1);
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }
}
