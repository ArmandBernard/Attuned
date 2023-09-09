using AttunedWebApi.Dtos;
using iTunesSmartParser.Data;
using iTunesSmartParser.Xml;
using Microsoft.AspNetCore.Mvc;

namespace AttunedWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TrackController : ControllerBase
{
    private readonly ILogger<TrackController> _logger;
    private readonly IXmlParser _xmlParser;

    public TrackController(ILogger<TrackController> logger, IXmlParser xmlParser)
    {
        _logger = logger;
        _xmlParser = xmlParser;
    }

    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrackDto>))]
    public async Task<IActionResult> Get()
    {
        return Ok(await _xmlParser.ParseTracks());
    }
}