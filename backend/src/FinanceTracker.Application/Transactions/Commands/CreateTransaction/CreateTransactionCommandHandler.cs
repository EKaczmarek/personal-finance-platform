using AutoMapper;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken ct)
    {
        var account = await accountRepository.GetByIdAsync(request.AccountId, request.UserId, ct)
            ?? throw new KeyNotFoundException($"Account {request.AccountId} not found.");

        var transaction = Transaction.Create(
            request.UserId, request.AccountId, request.CategoryId,
            request.Amount, request.Type, request.Description, request.OccurredAt);

        account.ApplyTransaction(request.Amount, request.Type);

        await transactionRepository.AddAsync(transaction, ct);
        accountRepository.Update(account);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<TransactionDto>(transaction);
    }
}
