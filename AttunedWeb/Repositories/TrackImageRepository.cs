using AttunedWebApi.Dtos;
using iTunesSmartParser.Xml;

namespace AttunedWebApi.Repositories;

public class TrackImageRepository(IXmlSource xmlSource, ITrackListParser trackListParser)
    : IRepository<ImageDto, string>
{
    public async Task<IEnumerable<ImageDto>> Get(CancellationToken token)
    {
        var doc = await xmlSource.GetXDocument(token);

        var dictionary = trackListParser.GetAllImages(doc);
        
        return dictionary.Select(x => new ImageDto(Convert.ToBase64String(x.Key), x.Value));
    }

    public async Task<string?> GetById(int id, CancellationToken token)
    {
        var doc = await xmlSource.GetXDocument(token);

        var image = trackListParser.GetById(doc, id)?.CoverArt;

        return image != null ? Convert.ToBase64String(image) : null;
    }
}