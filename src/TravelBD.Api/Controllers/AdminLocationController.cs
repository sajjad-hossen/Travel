using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelBD.Application.DTOs;
using TravelBD.Application.Interfaces;

namespace TravelBD.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/v1/admin/locations")]
public class AdminLocationController : ControllerBase
{
    private readonly IAdminLocationManager _manager;

    public AdminLocationController(IAdminLocationManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var locations = await _manager.GetAllLocationsAsync(ct);
        return Ok(locations);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var loc = await _manager.GetLocationByIdAsync(id, ct);
        return loc == null ? NotFound(new { message = "Location not found." }) : Ok(loc);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLocationRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest(new { message = "Name is required." });

        var created = await _manager.CreateLocationAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLocationRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest(new { message = "Name is required." });

        var updated = await _manager.UpdateLocationAsync(id, request, ct);
        return updated == null ? NotFound(new { message = "Location not found." }) : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        bool success = await _manager.DeleteLocationAsync(id, ct);
        return success ? NoContent() : NotFound(new { message = "Location not found." });
    }

    // ── Attractions ───────────────────────────────────────────────────────────
    [HttpPost("attractions")]
    public async Task<IActionResult> AddAttraction([FromBody] CreateAttractionRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest(new { message = "Attraction name is required." });

        var created = await _manager.AddAttractionAsync(request, ct);
        return Created($"/api/v1/admin/locations/attractions/{created.Id}", created);
    }

    [HttpPut("attractions/{id:guid}")]
    public async Task<IActionResult> UpdateAttraction(Guid id, [FromBody] UpdateAttractionRequest request, CancellationToken ct)
    {
        var updated = await _manager.UpdateAttractionAsync(id, request, ct);
        return updated == null ? NotFound(new { message = "Attraction not found." }) : Ok(updated);
    }

    [HttpDelete("attractions/{id:guid}")]
    public async Task<IActionResult> DeleteAttraction(Guid id, CancellationToken ct)
    {
        bool success = await _manager.DeleteAttractionAsync(id, ct);
        return success ? NoContent() : NotFound(new { message = "Attraction not found." });
    }

    // ── Accommodations ────────────────────────────────────────────────────────
    [HttpPost("accommodations")]
    public async Task<IActionResult> AddAccommodation([FromBody] CreateAccommodationRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest(new { message = "Accommodation name is required." });

        var created = await _manager.AddAccommodationAsync(request, ct);
        return Created($"/api/v1/admin/locations/accommodations/{created.Id}", created);
    }

    [HttpPut("accommodations/{id:guid}")]
    public async Task<IActionResult> UpdateAccommodation(Guid id, [FromBody] UpdateAccommodationRequest request, CancellationToken ct)
    {
        var updated = await _manager.UpdateAccommodationAsync(id, request, ct);
        return updated == null ? NotFound(new { message = "Accommodation not found." }) : Ok(updated);
    }

    [HttpDelete("accommodations/{id:guid}")]
    public async Task<IActionResult> DeleteAccommodation(Guid id, CancellationToken ct)
    {
        bool success = await _manager.DeleteAccommodationAsync(id, ct);
        return success ? NoContent() : NotFound(new { message = "Accommodation not found." });
    }

    // ── Advisories ────────────────────────────────────────────────────────────
    [HttpPost("advisories")]
    public async Task<IActionResult> AddAdvisory([FromBody] CreateAdvisoryRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Content))
            return BadRequest(new { message = "Title and Content are required." });

        var created = await _manager.AddAdvisoryAsync(request, ct);
        return Created($"/api/v1/admin/locations/advisories/{created.Id}", created);
    }

    [HttpPut("advisories/{id:guid}")]
    public async Task<IActionResult> UpdateAdvisory(Guid id, [FromBody] UpdateAdvisoryRequest request, CancellationToken ct)
    {
        var updated = await _manager.UpdateAdvisoryAsync(id, request, ct);
        return updated == null ? NotFound(new { message = "Advisory not found." }) : Ok(updated);
    }

    [HttpDelete("advisories/{id:guid}")]
    public async Task<IActionResult> DeleteAdvisory(Guid id, CancellationToken ct)
    {
        bool success = await _manager.DeleteAdvisoryAsync(id, ct);
        return success ? NoContent() : NotFound(new { message = "Advisory not found." });
    }
}
