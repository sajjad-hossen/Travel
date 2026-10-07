using TravelBD.Domain.Enums;

namespace TravelBD.Application.DTOs;

public record RegisterRequest(
    string Name,
    string Email,
    string Password
);

public record LoginRequest(
    string Email,
    string Password
);

public record AuthResponse(
    string Token,
    Guid UserId,
    string Name,
    string Email,
    UserRole Role
);
