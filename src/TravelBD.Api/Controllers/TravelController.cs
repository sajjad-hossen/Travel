using Microsoft.AspNetCore.Mvc;
using TravelBD.Application.Common.Models;
using TravelBD.Application.Interfaces;

namespace TravelBD.Api.Controllers;

[ApiController]
[Route("api/v1")]
public class TravelController : ControllerBase
{
    private readonly ITravelDataService _dataService;
    private readonly IRoutePlannerService _routePlanner;

    public TravelController(ITravelDataService dataService, IRoutePlannerService routePlanner)
    {
        _dataService = dataService;
        _routePlanner = routePlanner;
    }

    /// <summary>
    /// Search routes between any two locations with multi-hop options and transport details.
    /// </summary>
    [HttpGet("search/routes")]
    [ProducesResponseType(typeof(RouteSearchResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchRoutes(
        [FromQuery] string from,
        [FromQuery] string to,
        [FromQuery] string? pref = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
        {
            return BadRequest(new { message = "Both 'from' and 'to' query parameters are required." });
        }

        var result = await _routePlanner.PlanRouteAsync(from, to, pref, cancellationToken);
        if (result == null)
        {
            return NotFound(new { message = $"Could not resolve route from '{from}' to '{to}'." });
        }

        return Ok(result);
    }

    /// <summary>
    /// Autocomplete / search locations across districts, transit hubs, and tourist destinations.
    /// </summary>
    [HttpGet("locations/search")]
    public async Task<IActionResult> SearchLocations([FromQuery] string? q, CancellationToken cancellationToken)
    {
        var locations = await _dataService.SearchLocationsAsync(q ?? string.Empty, cancellationToken);
        return Ok(locations);
    }

    /// <summary>
    /// Get popular tourist destination hubs (Chittagong, Cox's Bazar, Bandarban, Rangamati, etc.)
    /// </summary>
    [HttpGet("locations/destinations")]
    public async Task<IActionResult> GetDestinations(CancellationToken cancellationToken)
    {
        var destinations = await _dataService.GetTouristDestinationsAsync(cancellationToken);
        return Ok(destinations);
    }

    /// <summary>
    /// Get full destination guide: attractions, stays, food, and hill tracts advisories/permits.
    /// </summary>
    [HttpGet("destinations/{slug}")]
    [ProducesResponseType(typeof(DestinationDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDestinationDetails(string slug, CancellationToken cancellationToken)
    {
        var details = await _dataService.GetDestinationDetailsAsync(slug, cancellationToken);
        if (details == null)
        {
            return NotFound(new { message = $"Destination '{slug}' not found." });
        }

        return Ok(details);
    }
}
