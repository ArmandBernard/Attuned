using AttunedWebApi.Dtos.Playlists;
using AttunedWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AttunedWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PlaylistController(IRepository<PlaylistDto, PlaylistDetailsDto> playlistRepository) : ControllerBase
{
    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PlaylistDto>))]
    public async Task<IActionResult> Get(CancellationToken token)
    {
        return Ok(await playlistRepository.Get(token));
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlaylistDetailsDto))]
    public async Task<IActionResult> Get(int id, CancellationToken token)
    {
        var playlist = await playlistRepository.GetById(id, token);

        return playlist != null ? Ok(playlist) : NotFound();
    }
}