using TravelBD.Domain.Entities;

namespace TravelBD.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
