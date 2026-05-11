using FinanceTracker.Application.DTOs;
using MediatR;

namespace FinanceTracker.Application.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string Token) : IRequest<AuthResponseDto>;
