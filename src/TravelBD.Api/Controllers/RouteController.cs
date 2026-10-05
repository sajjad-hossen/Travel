using Microsoft.AspNetCore.Mvc;
using TravelBD.Application.DTOs;
using TravelBD.Application.Interfaces;

namespace TravelBD.Api.Controllers;

[ApiController]
[Route("api/v1/routes")]
public class RouteController : ControllerBase
{
    private readonly IRouteManager _routeManager;

    public RouteController(IRouteManager routeManager)
    {
        _routeManager = routeManager;
    }

    /// <summary>
    /// Search routes between any two locations with multi-hop options and transport details.
    /// </summary>
    /// <param name="from">Origin location name, slug, district, or Guid.</param>
    /// <param name="to">Destination location name, slug, district, or Guid.</param>
    /// <param name="pref">Sorting preference: "fastest", "cheapest", or omit for recommended.</param>
    [HttpGet("search")]
    [HttpGet("/api/v1/search/routes")]
    [ProducesResponseType(typeof(RouteSearchResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchRoutes(
        [FromQuery] string from,
        [FromQuery] string to,
        [FromQuery] string? pref = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
            return BadRequest(new { message = "Both 'from' and 'to' query parameters are required." });

        var result = await _routeManager.PlanRouteAsync(from, to, pref, cancellationToken);
        if (result == null)
            return NotFound(new { message = $"Could not resolve route from '{from}' to '{to}'." });

        return Ok(result);
    }
}
