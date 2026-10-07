using TravelBD.Application.DTOs;
using TravelBD.Application.Interfaces;
using TravelBD.Application.Security;
using TravelBD.Domain.Entities;
using TravelBD.Domain.Enums;

namespace TravelBD.Application.Managers;

public class AuthManager : IAuthManager
{
    private readonly IUserRepository _users;
    private readonly IJwtTokenService _tokenService;

    public AuthManager(IUserRepository users, IJwtTokenService tokenService)
    {
        _users = users;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var existing = await _users.GetByEmailAsync(email, ct);
        if (existing != null)
            throw new InvalidOperationException("An account with this email already exists.");

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = email,
            PasswordHash = PasswordHasher.Hash(request.Password),
            Role = UserRole.Customer
        };

        await _users.AddAsync(user, ct);
        await _users.SaveChangesAsync(ct);

        var token = _tokenService.GenerateToken(user);
        return new AuthResponse(token, user.Id, user.Name, user.Email, user.Role);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _users.GetByEmailAsync(email, ct);

        if (user == null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var token = _tokenService.GenerateToken(user);
        return new AuthResponse(token, user.Id, user.Name, user.Email, user.Role);
    }
}
