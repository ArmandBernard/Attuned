using AttunedWebApi.Dtos;
using iTunesSmartParser.Xml;

namespace AttunedWebApi.Repositories;

public class TrackImageResizeRepository(IXmlSource xmlSource, ITrackListParser trackListParser, int size, int quality)
    : IRepository<ImageDto, string>
{
    public async Task<IEnumerable<ImageDto>> Get(CancellationToken token)
    {
        var doc = await xmlSource.GetXDocument(token);

        var dictionary = trackListParser.GetAllImages(doc, bytes => Utils.ImageUtils.ResizeImage(bytes, size, quality));

        return dictionary.Select(x => new ImageDto(Convert.ToBase64String(x.Key), x.Value));
    }

    public async Task<string?> GetById(int id, CancellationToken token)
    {
        var doc = await xmlSource.GetXDocument(token);

        var image = trackListParser.GetById(doc, id)?.CoverArt;

        if (image == null)
        {
            return null;
        }
        
        try
        {
            image = Utils.ImageUtils.ResizeImage(image, size, quality);
        }
        catch
        {
            // image may fail to load or be resized, but we can still pass on the unaltered bytes.
        }

        return Convert.ToBase64String(image);
    }
}