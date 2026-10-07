using TravelBD.Application.DTOs;

namespace TravelBD.Application.Interfaces;

public interface IAuthManager
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct);
}
