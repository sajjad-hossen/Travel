using TravelBD.Application.DTOs;
using TravelBD.Domain.Entities;

namespace TravelBD.Application.Interfaces;

/// <summary>
/// Repository contract for location data access.
/// </summary>
public interface ILocationRepository
{
    Task<List<LocationDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<LocationDto>> SearchAsync(string query, CancellationToken cancellationToken = default);
    Task<List<LocationDto>> GetTouristDestinationsAsync(CancellationToken cancellationToken = default);
    Task<DestinationDetailDto?> GetDetailsBySlugOrIdAsync(string slugOrId, CancellationToken cancellationToken = default);
    Task<Attraction?> GetAttractionDetailAsync(Guid attractionId, CancellationToken cancellationToken = default);
}
