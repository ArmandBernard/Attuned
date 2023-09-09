using AttunedWebApi.Dtos;
using iTunesSmartParser.Xml;
using Microsoft.AspNetCore.Mvc;

namespace AttunedWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PlaylistController : ControllerBase
{
    private readonly ILogger<PlaylistController> _logger;
    private readonly IXmlParser _xmlParser;

    public PlaylistController(ILogger<PlaylistController> logger, IXmlParser xmlParser)
    {
        _logger = logger;
        _xmlParser = xmlParser;
    }

    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrackDto>))]
    public async Task<IActionResult> Get()
    {
        return Ok((await _xmlParser.ParsePlaylists()).Where(x => x.Name != "Downloaded" && x.Name != "Library")
            .Select(PlaylistDto.FromPlaylist));
    }
}