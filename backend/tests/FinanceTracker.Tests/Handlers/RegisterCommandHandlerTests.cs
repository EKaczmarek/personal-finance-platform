using FinanceTracker.Application.Auth.Commands.Register;
using FinanceTracker.Application.Common.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using Moq;

namespace FinanceTracker.Tests.Handlers;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly Mock<IRefreshTokenRepository> _refreshRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IPasswordHasher> _hasher = new();
    private readonly Mock<ITokenService> _tokens = new();

    private RegisterCommandHandler CreateHandler() =>
        new(_userRepo.Object, _refreshRepo.Object, _uow.Object, _hasher.Object, _tokens.Object);

    [Fact]
    public async Task Handle_WhenEmailAlreadyExists_ThrowsInvalidOperationException()
    {
        _userRepo.Setup(r => r.GetByEmailAsync("taken@example.com", default))
                 .ReturnsAsync(User.Create("taken@example.com", "hash", "John", "Doe"));

        var cmd = new RegisterCommand("taken@example.com", "password123", "Jane", "Doe");

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => CreateHandler().Handle(cmd, default));
    }

    [Fact]
    public async Task Handle_WhenValidInput_ReturnsAuthResponse()
    {
        _userRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>(), default))
                 .ReturnsAsync((User?)null);
        _hasher.Setup(h => h.Hash(It.IsAny<string>())).Returns("hashed");
        _tokens.Setup(t => t.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>()))
               .Returns("access-token");
        _tokens.Setup(t => t.GenerateRefreshToken()).Returns("refresh-token");

        var cmd = new RegisterCommand("new@example.com", "password123", "Jane", "Doe");
        var result = await CreateHandler().Handle(cmd, default);

        Assert.Equal("access-token", result.AccessToken);
        Assert.Equal("new@example.com", result.Email);
        Assert.Equal("Jane", result.FirstName);
    }
}
