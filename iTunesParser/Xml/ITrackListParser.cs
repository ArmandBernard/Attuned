using System.Xml.Linq;

namespace iTunesSmartParser.Xml;

public interface ITrackListParser
{
    IEnumerable<Track> LoadTracks(XDocument doc);
}