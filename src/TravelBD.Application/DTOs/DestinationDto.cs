namespace TravelBD.Application.DTOs;

public record AttractionDto(
    Guid Id,
    string Name,
    string? BanglaName,
    string? Description,
    string? BestTimeToVisit,
    decimal EntryFeeBdt,
    string? ImageUrl,
    string? Category
);

public record AccommodationDto(
    Guid Id,
    string Name,
    string BudgetLevel,
    string ApproxPriceRange,
    string? Address,
    string? ContactPhone,
    string? BookingUrl,
    double Rating,
    string? HighlightFeature
);

public record AdvisoryDto(
    Guid Id,
    string Category,
    string Title,
    string Content,
    bool IsMandatory
);

public record DestinationDetailDto(
    LocationDto Location,
    List<AttractionDto> Attractions,
    List<AccommodationDto> Accommodations,
    List<AdvisoryDto> Advisories
);
