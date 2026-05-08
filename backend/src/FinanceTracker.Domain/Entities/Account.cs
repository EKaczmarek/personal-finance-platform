using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Account : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public AccountType Type { get; private set; }
    public decimal Balance { get; private set; }
    public string Currency { get; private set; } = "PLN";
    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;

    private readonly List<Transaction> _transactions = [];
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    private Account() { }

    public static Account Create(Guid userId, string name, AccountType type, decimal initialBalance, string currency = "PLN")
    {
        return new Account
        {
            UserId = userId,
            Name = name,
            Type = type,
            Balance = initialBalance,
            Currency = currency
        };
    }

    public void ApplyTransaction(decimal amount, TransactionType type)
    {
        Balance += type == TransactionType.Income ? amount : -amount;
        SetUpdatedAt();
    }

    public void UpdateName(string name)
    {
        Name = name;
        SetUpdatedAt();
    }
}
