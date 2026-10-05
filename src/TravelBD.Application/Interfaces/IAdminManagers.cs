using TravelBD.Application.DTOs;

namespace TravelBD.Application.Interfaces;

public interface IAdminLocationManager
{
    Task<List<LocationDto>> GetAllLocationsAsync(CancellationToken cancellationToken = default);
    Task<LocationDto?> GetLocationByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<LocationDto> CreateLocationAsync(CreateLocationRequest request, CancellationToken cancellationToken = default);
    Task<LocationDto?> UpdateLocationAsync(Guid id, UpdateLocationRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteLocationAsync(Guid id, CancellationToken cancellationToken = default);

    // Attractions
    Task<AttractionDto> AddAttractionAsync(CreateAttractionRequest request, CancellationToken cancellationToken = default);
    Task<AttractionDto?> UpdateAttractionAsync(Guid id, UpdateAttractionRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAttractionAsync(Guid id, CancellationToken cancellationToken = default);

    // Accommodations
    Task<AccommodationDto> AddAccommodationAsync(CreateAccommodationRequest request, CancellationToken cancellationToken = default);
    Task<AccommodationDto?> UpdateAccommodationAsync(Guid id, UpdateAccommodationRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAccommodationAsync(Guid id, CancellationToken cancellationToken = default);

    // Advisories
    Task<AdvisoryDto> AddAdvisoryAsync(CreateAdvisoryRequest request, CancellationToken cancellationToken = default);
    Task<AdvisoryDto?> UpdateAdvisoryAsync(Guid id, UpdateAdvisoryRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAdvisoryAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IAdminRouteManager
{
    Task<List<AdminRouteSegmentDto>> GetAllSegmentsAsync(CancellationToken cancellationToken = default);
    Task<AdminRouteSegmentDto?> GetSegmentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AdminRouteSegmentDto> CreateSegmentAsync(CreateRouteSegmentRequest request, CancellationToken cancellationToken = default);
    Task<AdminRouteSegmentDto?> UpdateSegmentAsync(Guid id, UpdateRouteSegmentRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteSegmentAsync(Guid id, CancellationToken cancellationToken = default);

    // Transport Options
    Task<TransportOptionDto> AddTransportOptionAsync(CreateTransportOptionRequest request, CancellationToken cancellationToken = default);
    Task<TransportOptionDto?> UpdateTransportOptionAsync(Guid id, UpdateTransportOptionRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteTransportOptionAsync(Guid id, CancellationToken cancellationToken = default);
}
