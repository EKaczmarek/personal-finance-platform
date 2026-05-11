using FinanceTracker.Application.Common.Interfaces;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Interfaces;
using MediatR;
using RefreshTokenEntity = FinanceTracker.Domain.Entities.RefreshToken;

namespace FinanceTracker.Application.Auth.Commands.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : IRequestHandler<LoginCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, ct)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var accessToken = tokenService.GenerateAccessToken(user.Id, user.Email);
        var rawRefresh = tokenService.GenerateRefreshToken();
        var refreshToken = RefreshTokenEntity.Create(user.Id, rawRefresh, DateTime.UtcNow.AddDays(7));
        await refreshTokenRepository.AddAsync(refreshToken, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return new AuthResponseDto(accessToken, rawRefresh, user.Email, user.FirstName, user.LastName);
    }
}
