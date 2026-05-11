using FinanceTracker.Application.DTOs;
using MediatR;

namespace FinanceTracker.Application.Auth.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : IRequest<AuthResponseDto>;
