using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public interface ITrackListParser
{
    IEnumerable<Track> ParseTracks(XDocument doc);
    Track ParseTrackElement(XElement playlistElement);
}