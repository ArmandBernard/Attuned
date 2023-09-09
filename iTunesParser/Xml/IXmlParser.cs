using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public interface IXmlParser
{
    Task<IEnumerable<Track>> ParseTracks(string path, CancellationToken? token = null);

    Task<IEnumerable<Playlist>> ParsePlaylists(string path, CancellationToken? token = null);
}