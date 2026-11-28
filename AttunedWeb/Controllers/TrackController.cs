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
        return Ok(await trackRepository.GetById(id, token));
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
    public async Task<IActionResult> Patch(int id, TrackPatchDto patch, CancellationToken token)
    {
        var track = await trackWriter.Patch(id, patch.ToPatch(), token);

        return track == null ? NotFound() : Ok(track);
    }
}
