using AutoMapper;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler(
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateAccountCommand, AccountDto>
{
    public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken ct)
    {
        var account = Account.Create(request.UserId, request.Name, request.Type, request.InitialBalance, request.Currency);
        await accountRepository.AddAsync(account, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return mapper.Map<AccountDto>(account);
    }
}
