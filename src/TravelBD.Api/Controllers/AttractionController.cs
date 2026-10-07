using Microsoft.AspNetCore.Mvc;
using TravelBD.Application.Interfaces;

namespace TravelBD.Api.Controllers;

[ApiController]
[Route("api/v1/attractions")]
public class AttractionController : ControllerBase
{
    private readonly IAttractionManager _manager;

    public AttractionController(IAttractionManager manager)
    {
        _manager = manager;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var detail = await _manager.GetDetailAsync(id, ct);
        return detail == null ? NotFound(new { message = "Attraction not found." }) : Ok(detail);
    }
}
