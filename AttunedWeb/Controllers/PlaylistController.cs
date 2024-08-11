using AttunedWebApi.Dtos;
using iTunesSmartParser.Xml;
using Microsoft.AspNetCore.Mvc;

namespace AttunedWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PlaylistController(ILogger<PlaylistController> logger, IXmlParser xmlParser) : ControllerBase
{
    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrackDto>))]
    public async Task<IActionResult> Get()
    {
        return Ok((await xmlParser.ParsePlaylists()).Where(x => x.Name != "Downloaded" && x.Name != "Library")
            .Select(PlaylistDto.FromPlaylist));
    }
    
    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrackDto>))]
    public async Task<IActionResult> Get(int id)
    {
        var playlist = (await xmlParser.ParsePlaylists()).FirstOrDefault(x => x.Id == id);

        return playlist == null ? NotFound() : Ok(PlaylistDto.FromPlaylist(playlist));
    }
}