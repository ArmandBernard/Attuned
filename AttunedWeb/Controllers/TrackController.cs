using AttunedWebApi.Dtos;
using AttunedWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AttunedWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TrackController(IRepository<TrackDto, TrackDto> trackRepository)
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrackDto))]
    public async Task<IActionResult> GetById(int id, CancellationToken token)
    {
        return Ok(await trackRepository.GetById(id, token));
    }
}