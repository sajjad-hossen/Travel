namespace TravelBD.Application.DTOs;

public record LocationDto(
    Guid Id,
    string Name,
    string? BanglaName,
    string Slug,
    string Type,
    string District,
    string Division,
    double? Latitude,
    double? Longitude,
    bool IsMajorHub,
    bool IsTouristDestination,
    string? Description,
    string? HeroImageUrl
);
