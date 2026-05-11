using FinanceTracker.Application.Common.Interfaces;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Interfaces;
using MediatR;
using RefreshTokenEntity = FinanceTracker.Domain.Entities.RefreshToken;
using UserEntity = FinanceTracker.Domain.Entities.User;

namespace FinanceTracker.Application.Auth.Commands.Register;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken ct)
    {
        var existing = await userRepository.GetByEmailAsync(request.Email, ct);
        if (existing is not null)
            throw new InvalidOperationException($"Email '{request.Email}' is already registered.");

        var hash = passwordHasher.Hash(request.Password);
        var user = UserEntity.Create(request.Email, hash, request.FirstName, request.LastName);
        await userRepository.AddAsync(user, ct);

        var accessToken = tokenService.GenerateAccessToken(user.Id, user.Email);
        var rawRefresh = tokenService.GenerateRefreshToken();
        var refreshToken = RefreshTokenEntity.Create(user.Id, rawRefresh, DateTime.UtcNow.AddDays(7));
        await refreshTokenRepository.AddAsync(refreshToken, ct);

        await unitOfWork.SaveChangesAsync(ct);

        return new AuthResponseDto(accessToken, rawRefresh, user.Email, user.FirstName, user.LastName);
    }
}
