using TravelBD.Application.DTOs;
using TravelBD.Application.Interfaces;

namespace TravelBD.Application.Managers;

/// <summary>
/// Handles location business logic. Coordinates with ILocationRepository for data access.
/// </summary>
public class LocationManager : ILocationManager
{
    private readonly ILocationRepository _locationRepository;

    public LocationManager(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public Task<List<LocationDto>> GetAllLocationsAsync(CancellationToken cancellationToken = default)
        => _locationRepository.GetAllAsync(cancellationToken);

    public Task<List<LocationDto>> SearchLocationsAsync(string query, CancellationToken cancellationToken = default)
        => _locationRepository.SearchAsync(query, cancellationToken);

    public Task<List<LocationDto>> GetTouristDestinationsAsync(CancellationToken cancellationToken = default)
        => _locationRepository.GetTouristDestinationsAsync(cancellationToken);

    public Task<DestinationDetailDto?> GetDestinationDetailsAsync(string slugOrId, CancellationToken cancellationToken = default)
        => _locationRepository.GetDetailsBySlugOrIdAsync(slugOrId, cancellationToken);
}
