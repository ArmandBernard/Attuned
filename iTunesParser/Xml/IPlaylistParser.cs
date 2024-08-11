using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public interface IPlaylistParser
{
    IEnumerable<Playlist> ParseDocument(XDocument doc);
    PlaylistDetails ParsePlaylistDetails(XElement playlistElement);
    PlaylistDetails? GetById(XDocument doc, int id);
}