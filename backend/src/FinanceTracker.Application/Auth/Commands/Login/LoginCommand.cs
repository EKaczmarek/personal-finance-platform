using FinanceTracker.Application.DTOs;
using MediatR;

namespace FinanceTracker.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;
