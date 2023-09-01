using Microsoft.AspNetCore.Mvc;

namespace AttunedWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TracksController : ControllerBase
{
    private readonly ILogger<TracksController> _logger;

    public TracksController(ILogger<TracksController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("")]
    public IEnumerable<WeatherForecast> Get()
    {

    }
}
