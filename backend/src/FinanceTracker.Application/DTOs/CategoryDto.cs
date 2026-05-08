namespace FinanceTracker.Application.DTOs;

public record CategoryDto(
    Guid Id,
    string Name,
    string Icon,
    string Color);
