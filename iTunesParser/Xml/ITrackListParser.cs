using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public interface ITrackListParser
{
    IEnumerable<Track> ParseDocument(XDocument doc);
    Track ParseTrackElement(XElement tracksElement);
}