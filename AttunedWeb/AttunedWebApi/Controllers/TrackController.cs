using iTunesSmartParser.Data;
using Microsoft.AspNetCore.Mvc;

namespace AttunedWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TrackController : ControllerBase
{
    private readonly ILogger<TrackController> _logger;

    public TrackController(ILogger<TrackController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("")]
    public IEnumerable<Track> Get()
    {
        return new[]
        {
            new Track()
            {
                Id = 1,
                Name = "Name",
                Location = "Nowhere",
                Album = "Album",
                Artist = "Artist",
                Bpm = 128,
                Genre = "Genre",
                Channels = 2,
                Composer = "Composer",
                DateAdded = DateTime.Today,
                DateModified = DateTime.UtcNow,
                Year = 1980,
                DiscCount = 1,
                TrackCount = 4,
                DiscNumber = 1,
                Rating = 3,
                Size = 4000
            }
        };
    }
}