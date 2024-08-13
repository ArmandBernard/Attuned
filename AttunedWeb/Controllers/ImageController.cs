using AttunedWebApi.Dtos;
using AttunedWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AttunedWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController(IRepository<ImageDto, string> trackImageRepository)
    : ControllerBase
{
    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ImageDto>))]
    public async Task<IActionResult> Get(CancellationToken token)
    {
        return Ok(await trackImageRepository.Get(token));
    }
}