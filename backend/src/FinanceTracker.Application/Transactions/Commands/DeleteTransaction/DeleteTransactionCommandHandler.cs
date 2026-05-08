using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteTransactionCommand>
{
    public async Task Handle(DeleteTransactionCommand request, CancellationToken ct)
    {
        var transaction = await transactionRepository.GetByIdAsync(request.TransactionId, request.UserId, ct)
            ?? throw new KeyNotFoundException($"Transaction {request.TransactionId} not found.");

        transactionRepository.Remove(transaction);
        await unitOfWork.SaveChangesAsync(ct);
    }
}
