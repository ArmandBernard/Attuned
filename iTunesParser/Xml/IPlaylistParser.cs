using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public interface IPlaylistParser
{
    IEnumerable<Playlist> ParseDocument(XDocument doc);
    Playlist ParsePlaylistElement(XElement playlistElement);
    Playlist? GetById(XDocument doc, int id);
}