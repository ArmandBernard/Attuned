using AttunedWebApi.Dtos.Tracks;
using AttunedWebApi.Filters;
using AttunedWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AttunedWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TrackController(IRepository<TrackDto, TrackDetailsDto> trackRepository, ITrackWriter trackWriter)
    : ControllerBase
{
    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrackDto>))]
    public async Task<IActionResult> Get(CancellationToken token)
    {
        return Ok(await trackRepository.Get(token));
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrackDetailsDto))]
    public async Task<IActionResult> GetById(int id, CancellationToken token)
    {
        return Ok(await trackRepository.GetDetailsById(id, token));
    }

    /// <summary>
    /// Update only the properties that are provided.
    /// </summary>
    [HttpPatch]
    [Route("{id:int}")]
    [RequireWritable]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrackDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Patch(int id, TrackPatchDto patchDto, CancellationToken token)
    {
        var patch = patchDto.ToPatch();
        
        if (patch.IsEmpty)
        {
            // nothing to change, so there is no need to write anything. Just say how the track is.
            var existing = await trackRepository.GetById(id, token);

            return existing == null ? NotFound() : Ok(existing);
        }

        var track = await trackWriter.Patch(id, patch, token);

        return track == null ? NotFound() : Ok(track);
    }
}
