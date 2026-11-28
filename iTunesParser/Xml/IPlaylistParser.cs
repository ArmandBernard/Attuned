using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public interface IPlaylistParser
{
    IEnumerable<Playlist> ParseDocument(XDocument doc);
    PlaylistDetails ParsePlaylistDetails(XElement playlistElement);
    Playlist? GetById(XDocument doc, int id);
    PlaylistDetails? GetDetailsById(XDocument doc, int id);
}