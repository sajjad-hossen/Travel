using Microsoft.AspNetCore.Mvc;
using TravelBD.Api.Filters;
using TravelBD.Application.DTOs;
using TravelBD.Application.Interfaces;

namespace TravelBD.Api.Controllers;

[ApiController]
[AdminApiKey]
[Route("api/v1/admin/routes")]
public class AdminRouteController : ControllerBase
{
    private readonly IAdminRouteManager _manager;

    public AdminRouteController(IAdminRouteManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var segments = await _manager.GetAllSegmentsAsync(ct);
        return Ok(segments);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var segment = await _manager.GetSegmentByIdAsync(id, ct);
        return segment == null ? NotFound(new { message = "Route segment not found." }) : Ok(segment);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRouteSegmentRequest request, CancellationToken ct)
    {
        try
        {
            var created = await _manager.CreateSegmentAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRouteSegmentRequest request, CancellationToken ct)
    {
        var updated = await _manager.UpdateSegmentAsync(id, request, ct);
        return updated == null ? NotFound(new { message = "Route segment not found." }) : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        bool success = await _manager.DeleteSegmentAsync(id, ct);
        return success ? NoContent() : NotFound(new { message = "Route segment not found." });
    }

    // ── Transport Options ─────────────────────────────────────────────────────
    [HttpPost("transport")]
    public async Task<IActionResult> AddTransportOption([FromBody] CreateTransportOptionRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.OperatorName))
            return BadRequest(new { message = "Operator name is required." });

        var created = await _manager.AddTransportOptionAsync(request, ct);
        return Created($"/api/v1/admin/routes/transport/{created.Id}", created);
    }

    [HttpPut("transport/{id:guid}")]
    public async Task<IActionResult> UpdateTransportOption(Guid id, [FromBody] UpdateTransportOptionRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.OperatorName))
            return BadRequest(new { message = "Operator name is required." });

        var updated = await _manager.UpdateTransportOptionAsync(id, request, ct);
        return updated == null ? NotFound(new { message = "Transport option not found." }) : Ok(updated);
    }

    [HttpDelete("transport/{id:guid}")]
    public async Task<IActionResult> DeleteTransportOption(Guid id, CancellationToken ct)
    {
        bool success = await _manager.DeleteTransportOptionAsync(id, ct);
        return success ? NoContent() : NotFound(new { message = "Transport option not found." });
    }
}
