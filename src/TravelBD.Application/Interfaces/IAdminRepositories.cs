using TravelBD.Application.DTOs;
using TravelBD.Domain.Entities;

namespace TravelBD.Application.Interfaces;

public interface IAdminLocationRepository
{
    Task<List<Location>> GetAllLocationsAsync(CancellationToken cancellationToken = default);
    Task<Location?> GetLocationByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Location?> GetLocationBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Location> CreateLocationAsync(Location location, CancellationToken cancellationToken = default);
    Task UpdateLocationAsync(Location location, CancellationToken cancellationToken = default);
    Task<bool> DeleteLocationAsync(Guid id, CancellationToken cancellationToken = default);

    // Attractions
    Task<Attraction> AddAttractionAsync(Attraction attraction, CancellationToken cancellationToken = default);
    Task<Attraction?> GetAttractionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAttractionAsync(Attraction attraction, CancellationToken cancellationToken = default);
    Task<bool> DeleteAttractionAsync(Guid id, CancellationToken cancellationToken = default);

    // Accommodations
    Task<Accommodation> AddAccommodationAsync(Accommodation accommodation, CancellationToken cancellationToken = default);
    Task<Accommodation?> GetAccommodationByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAccommodationAsync(Accommodation accommodation, CancellationToken cancellationToken = default);
    Task<bool> DeleteAccommodationAsync(Guid id, CancellationToken cancellationToken = default);

    // Advisories
    Task<DestinationAdvisory> AddAdvisoryAsync(DestinationAdvisory advisory, CancellationToken cancellationToken = default);
    Task<DestinationAdvisory?> GetAdvisoryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAdvisoryAsync(DestinationAdvisory advisory, CancellationToken cancellationToken = default);
    Task<bool> DeleteAdvisoryAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IAdminRouteRepository
{
    Task<List<RouteSegment>> GetAllSegmentsAsync(CancellationToken cancellationToken = default);
    Task<RouteSegment?> GetSegmentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> SegmentExistsAsync(Guid originId, Guid destinationId, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task<RouteSegment> CreateSegmentAsync(RouteSegment segment, CancellationToken cancellationToken = default);
    Task UpdateSegmentAsync(RouteSegment segment, CancellationToken cancellationToken = default);
    Task<bool> DeleteSegmentAsync(Guid id, CancellationToken cancellationToken = default);

    // Transport Options
    Task<TransportOption> AddTransportOptionAsync(TransportOption option, CancellationToken cancellationToken = default);
    Task<TransportOption?> GetTransportOptionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateTransportOptionAsync(TransportOption option, CancellationToken cancellationToken = default);
    Task<bool> DeleteTransportOptionAsync(Guid id, CancellationToken cancellationToken = default);
}
