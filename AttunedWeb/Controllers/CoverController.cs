using System.Security.Cryptography;
using iTunesSmartParser.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace AttunedWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CoverController(IXmlSource xmlSource, ITrackListParser trackListParser) : ControllerBase
{
    /// <summary>
    /// Get the cover art for a track as a raw image, so the browser can load, decode and cache it natively.
    /// </summary>
    [HttpGet]
    [Route("{trackId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByTrackId(int trackId, CancellationToken token)
    {
        var doc = await xmlSource.GetXDocument(token);

        var cover = trackListParser.GetCoverArt(doc, trackId);

        if (cover == null)
        {
            return NotFound();
        }

        // Tell the browser the hash of the file so it can cancel if it already has it in cache
        // (handy for tracks with the same album art)
        var etag = new EntityTagHeaderValue($"\"{Convert.ToHexString(SHA1.HashData(cover.Data))}\"");

        // prevent other queries for this resource for some time
        Response.Headers.CacheControl = "public, max-age=" + 10*1000;

        return File(cover.Data, cover.MimeType, lastModified: null, entityTag: etag);
    }
}
