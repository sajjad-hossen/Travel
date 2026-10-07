using Microsoft.AspNetCore.Mvc;
using TravelBD.Application.DTOs;
using TravelBD.Application.Interfaces;

namespace TravelBD.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthManager _manager;

    public AuthController(IAuthManager manager)
    {
        _manager = manager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { message = "Name, email and password are required." });

        if (request.Password.Length < 6)
            return BadRequest(new { message = "Password must be at least 6 characters." });

        try
        {
            var result = await _manager.RegisterAsync(request, ct);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { message = "Email and password are required." });

        try
        {
            var result = await _manager.LoginAsync(request, ct);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
