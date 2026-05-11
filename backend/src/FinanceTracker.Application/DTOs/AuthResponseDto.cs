namespace FinanceTracker.Application.DTOs;

public record AuthResponseDto(
    string AccessToken,
    string RefreshToken,
    string Email,
    string FirstName,
    string LastName);
