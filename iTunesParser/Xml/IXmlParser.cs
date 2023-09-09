using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public interface IXmlParser
{
    Task<IEnumerable<Track>> ParseTracks(CancellationToken? token = null);

    Task<IEnumerable<Playlist>> ParsePlaylists(CancellationToken? token = null);
}