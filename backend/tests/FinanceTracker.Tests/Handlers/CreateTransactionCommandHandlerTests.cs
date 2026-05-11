using AutoMapper;
using FinanceTracker.Application.Transactions.Commands.CreateTransaction;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Interfaces;
using Moq;

namespace FinanceTracker.Tests.Handlers;

public class CreateTransactionCommandHandlerTests
{
    private readonly Mock<ITransactionRepository> _txRepo = new();
    private readonly Mock<IAccountRepository> _accountRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IMapper> _mapper = new();

    private CreateTransactionCommandHandler CreateHandler() =>
        new(_txRepo.Object, _accountRepo.Object, _uow.Object, _mapper.Object);

    [Fact]
    public async Task Handle_WhenAccountNotFound_ThrowsKeyNotFoundException()
    {
        var userId = Guid.NewGuid();
        var accountId = Guid.NewGuid();

        _accountRepo.Setup(r => r.GetByIdAsync(accountId, userId, default))
                    .ReturnsAsync((Account?)null);

        var cmd = new CreateTransactionCommand(
            userId, accountId, Guid.NewGuid(), 100m, TransactionType.Expense, "Test", null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => CreateHandler().Handle(cmd, default));
    }
}
