using TravelBD.Application.DTOs;

namespace TravelBD.Application.Interfaces;

/// <summary>
/// Manager contract for location business logic.
/// </summary>
public interface ILocationManager
{
    Task<List<LocationDto>> GetAllLocationsAsync(CancellationToken cancellationToken = default);
    Task<List<LocationDto>> SearchLocationsAsync(string query, CancellationToken cancellationToken = default);
    Task<List<LocationDto>> GetTouristDestinationsAsync(CancellationToken cancellationToken = default);
    Task<DestinationDetailDto?> GetDestinationDetailsAsync(string slugOrId, CancellationToken cancellationToken = default);
}
