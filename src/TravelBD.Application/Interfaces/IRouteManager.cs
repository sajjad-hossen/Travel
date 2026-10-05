using TravelBD.Application.DTOs;

namespace TravelBD.Application.Interfaces;

/// <summary>
/// Manager contract for route planning business logic.
/// </summary>
public interface IRouteManager
{
    Task<RouteSearchResultDto?> PlanRouteAsync(
        string originQuery,
        string destinationQuery,
        string? preference = null,
        CancellationToken cancellationToken = default);
}
