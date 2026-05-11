using MediatR;

namespace FinanceTracker.Application.Auth.Commands.Logout;

public record LogoutCommand(string RefreshToken) : IRequest;
