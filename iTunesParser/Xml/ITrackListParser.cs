using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public interface ITrackListParser
{
    IEnumerable<Track> ParseDocument(XDocument doc);
    Track ParseTrackElement(XElement tracksElement);
    Track? GetById(XDocument doc, int id);
    TrackDetails? GetDetailsById(XDocument doc, int id);
    CoverArt? GetCoverArt(XDocument doc, int id);
}