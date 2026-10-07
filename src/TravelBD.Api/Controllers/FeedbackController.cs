using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelBD.Application.DTOs;
using TravelBD.Application.Interfaces;

namespace TravelBD.Api.Controllers;

[ApiController]
[Route("api/v1/feedback")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackManager _manager;

    public FeedbackController(IFeedbackManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? locationId, CancellationToken ct)
    {
        var feedbacks = await _manager.GetAllAsync(locationId, ct);
        return Ok(feedbacks);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFeedbackRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Comment))
            return BadRequest(new { message = "Comment is required." });

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Invalid session. Please log in again." });

        try
        {
            var created = await _manager.CreateAsync(userId, request, ct);
            return Ok(created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
