using TravelBD.Application.DTOs;
using TravelBD.Domain.Entities;

namespace TravelBD.Application.Interfaces;

/// <summary>
/// Repository contract for route segment data access.
/// </summary>
public interface IRouteRepository
{
    Task<List<RouteSegment>> GetAllActiveSegmentsAsync(CancellationToken cancellationToken = default);
    Task<Location?> ResolveLocationAsync(string query, CancellationToken cancellationToken = default);
}
