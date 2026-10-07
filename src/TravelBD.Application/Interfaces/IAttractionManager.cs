using TravelBD.Application.DTOs;

namespace TravelBD.Application.Interfaces;

public interface IAttractionManager
{
    Task<AttractionDetailDto?> GetDetailAsync(Guid attractionId, CancellationToken cancellationToken = default);
}
