using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public interface IPlaylistsParser
{
    IEnumerable<Playlist> ParseDocument(XDocument doc);
    Playlist ParsePlaylistElement(XElement playlistElement);
}