using AttunedWebApi.Dtos;
using iTunesSmartParser.Xml;
using Microsoft.AspNetCore.Mvc;

namespace AttunedWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TrackController(ILogger<TrackController> logger, IXmlParser xmlParser) : ControllerBase
{
    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrackDto>))]
    public async Task<IActionResult> Get()
    {
        return Ok((await xmlParser.ParseTracks()).Select(TrackDto.FromTrack));
    }
}