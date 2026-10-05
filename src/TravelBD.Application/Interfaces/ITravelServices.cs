using TravelBD.Application.Common.Models;

namespace TravelBD.Application.Interfaces;

public interface ITravelDataService
{
    Task<List<LocationDto>> GetAllLocationsAsync(CancellationToken cancellationToken = default);
    Task<List<LocationDto>> SearchLocationsAsync(string query, CancellationToken cancellationToken = default);
    Task<List<LocationDto>> GetTouristDestinationsAsync(CancellationToken cancellationToken = default);
    Task<DestinationDetailDto?> GetDestinationDetailsAsync(string slugOrId, CancellationToken cancellationToken = default);
}

public interface IRoutePlannerService
{
    Task<RouteSearchResultDto?> PlanRouteAsync(string originQuery, string destinationQuery, string? preference = null, CancellationToken cancellationToken = default);
}
