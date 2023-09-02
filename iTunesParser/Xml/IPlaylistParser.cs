using System.Xml.Linq;

namespace iTunesSmartParser.Xml;

public interface IPlaylistsParser
{
    IEnumerable<Playlist> ParsePlaylists(XDocument doc);
}