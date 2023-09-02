using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public interface IPlaylistsParser
{
    IEnumerable<Playlist> ParsePlaylists(XDocument doc);
}