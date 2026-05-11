using FinanceTracker.Application.Auth.Commands.Login;
using FinanceTracker.Application.Common.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using Moq;

namespace FinanceTracker.Tests.Handlers;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly Mock<IRefreshTokenRepository> _refreshRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IPasswordHasher> _hasher = new();
    private readonly Mock<ITokenService> _tokens = new();

    private LoginCommandHandler CreateHandler() =>
        new(_userRepo.Object, _refreshRepo.Object, _uow.Object, _hasher.Object, _tokens.Object);

    [Fact]
    public async Task Handle_WhenUserNotFound_ThrowsUnauthorizedAccessException()
    {
        _userRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>(), default))
                 .ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => CreateHandler().Handle(new LoginCommand("no@example.com", "pass"), default));
    }

    [Fact]
    public async Task Handle_WhenPasswordInvalid_ThrowsUnauthorizedAccessException()
    {
        _userRepo.Setup(r => r.GetByEmailAsync("user@example.com", default))
                 .ReturnsAsync(User.Create("user@example.com", "hash", "John", "Doe"));
        _hasher.Setup(h => h.Verify("wrongpass", "hash")).Returns(false);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => CreateHandler().Handle(new LoginCommand("user@example.com", "wrongpass"), default));
    }

    [Fact]
    public async Task Handle_WhenValidCredentials_ReturnsAuthResponse()
    {
        _userRepo.Setup(r => r.GetByEmailAsync("user@example.com", default))
                 .ReturnsAsync(User.Create("user@example.com", "hash", "John", "Doe"));
        _hasher.Setup(h => h.Verify("correctpass", "hash")).Returns(true);
        _tokens.Setup(t => t.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>()))
               .Returns("access-token");
        _tokens.Setup(t => t.GenerateRefreshToken()).Returns("refresh-token");

        var result = await CreateHandler().Handle(new LoginCommand("user@example.com", "correctpass"), default);

        Assert.Equal("access-token", result.AccessToken);
        Assert.Equal("user@example.com", result.Email);
    }
}
