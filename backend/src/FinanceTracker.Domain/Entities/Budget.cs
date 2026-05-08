using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.Entities;

public class Budget : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid CategoryId { get; private set; }
    public decimal MonthlyLimit { get; private set; }
    public int Month { get; private set; }
    public int Year { get; private set; }

    public User User { get; private set; } = null!;
    public Category Category { get; private set; } = null!;

    private Budget() { }

    public static Budget Create(Guid userId, Guid categoryId, decimal monthlyLimit, int month, int year)
    {
        return new Budget
        {
            UserId = userId,
            CategoryId = categoryId,
            MonthlyLimit = monthlyLimit,
            Month = month,
            Year = year
        };
    }

    public void UpdateLimit(decimal monthlyLimit)
    {
        MonthlyLimit = monthlyLimit;
        SetUpdatedAt();
    }
}
