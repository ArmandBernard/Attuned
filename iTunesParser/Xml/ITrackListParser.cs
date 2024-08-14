using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public interface ITrackListParser
{
    IEnumerable<Track> ParseDocument(XDocument doc);
    Track ParseTrackElement(XElement tracksElement);
    TrackDetails? GetById(XDocument doc, int id);
    Dictionary<byte[], HashSet<int>> GetAllImages(XDocument doc, Func<byte[], byte[]>? processImage = null);
}