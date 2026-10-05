using Microsoft.AspNetCore.Mvc;
using TravelBD.Application.DTOs;
using TravelBD.Application.Interfaces;

namespace TravelBD.Api.Controllers;

[ApiController]
[Route("api/v1/locations")]
public class LocationController : ControllerBase
{
    private readonly ILocationManager _locationManager;

    public LocationController(ILocationManager locationManager)
    {
        _locationManager = locationManager;
    }

    /// <summary>
    /// Autocomplete / search locations across districts, transit hubs, and tourist destinations.
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<LocationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchLocations(
        [FromQuery] string? q,
        CancellationToken cancellationToken)
    {
        var locations = await _locationManager.SearchLocationsAsync(q ?? string.Empty, cancellationToken);
        return Ok(locations);
    }

    /// <summary>
    /// Get popular tourist destination hubs (Chittagong, Cox's Bazar, Bandarban, Rangamati, etc.)
    /// </summary>
    [HttpGet("destinations")]
    [ProducesResponseType(typeof(List<LocationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDestinations(CancellationToken cancellationToken)
    {
        var destinations = await _locationManager.GetTouristDestinationsAsync(cancellationToken);
        return Ok(destinations);
    }

    /// <summary>
    /// Get full destination guide: attractions, stays, food, and hill tracts advisories/permits.
    /// </summary>
    [HttpGet("destinations/{slug}")]
    [HttpGet("/api/v1/destinations/{slug}")]
    [ProducesResponseType(typeof(DestinationDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDestinationDetails(
        string slug,
        CancellationToken cancellationToken)
    {
        var details = await _locationManager.GetDestinationDetailsAsync(slug, cancellationToken);
        if (details == null)
            return NotFound(new { message = $"Destination '{slug}' not found." });

        return Ok(details);
    }
}
