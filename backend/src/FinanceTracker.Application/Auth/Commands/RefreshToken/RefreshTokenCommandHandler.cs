using FinanceTracker.Application.Common.Interfaces;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Interfaces;
using MediatR;
using RefreshTokenEntity = FinanceTracker.Domain.Entities.RefreshToken;

namespace FinanceTracker.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork,
    ITokenService tokenService) : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var stored = await refreshTokenRepository.GetByTokenAsync(request.Token, ct)
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");

        if (!stored.IsValid())
            throw new UnauthorizedAccessException("Refresh token has expired or been revoked.");

        stored.Revoke();
        refreshTokenRepository.Update(stored);

        var user = stored.User;
        var accessToken = tokenService.GenerateAccessToken(user.Id, user.Email);
        var rawRefresh = tokenService.GenerateRefreshToken();
        var newRefresh = RefreshTokenEntity.Create(user.Id, rawRefresh, DateTime.UtcNow.AddDays(7));
        await refreshTokenRepository.AddAsync(newRefresh, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return new AuthResponseDto(accessToken, rawRefresh, user.Email, user.FirstName, user.LastName);
    }
}
