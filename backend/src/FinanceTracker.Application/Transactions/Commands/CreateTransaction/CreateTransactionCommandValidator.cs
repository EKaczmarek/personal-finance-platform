using FluentValidation;

namespace FinanceTracker.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
