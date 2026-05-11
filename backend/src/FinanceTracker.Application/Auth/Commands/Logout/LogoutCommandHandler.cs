using FinanceTracker.Domain.Interfaces;
using MediatR;

namespace FinanceTracker.Application.Auth.Commands.Logout;

public class LogoutCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken ct)
    {
        var token = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, ct);
        if (token is null || !token.IsValid())
            return;

        token.Revoke();
        refreshTokenRepository.Update(token);
        await unitOfWork.SaveChangesAsync(ct);
    }
}
