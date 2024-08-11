using AttunedWebApi.Dtos;
using AttunedWebApi.Repositories;
using iTunesSmartParser.Xml;
using Microsoft.AspNetCore.Mvc;

namespace AttunedWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PlaylistController(ILogger<PlaylistController> logger, IRepository<PlaylistDto> playlistRepository) : ControllerBase
{
    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PlaylistDto>))]
    public async Task<IActionResult> Get()
    {
        return Ok(await playlistRepository.Get());
    }
    
    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlaylistDto))]
    public async Task<IActionResult> Get(int id)
    {
        var playlist = (await playlistRepository.Get()).FirstOrDefault(x => x.Id == id);

        return playlist != null ? Ok(playlist) : NotFound();
    }
}