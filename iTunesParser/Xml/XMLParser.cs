using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public class XmlParser
{
    private ITrackListParser TrackListParser { get; }
    private IPlaylistsParser PlaylistsParser { get; }

    public XmlParser(ITrackListParser trackListParser, IPlaylistsParser playlistsParser)
    {

        TrackListParser = trackListParser;
        PlaylistsParser = playlistsParser;
    }

    /// <summary>
    /// Load all tracks from the iTunes library XML using the XDoc method
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Track>> ParseTracks(string path, CancellationToken? token = null) =>
        TrackListParser.ParseDocument(await OpenDocument(path, token ?? CancellationToken.None));

    public async Task<IEnumerable<Playlist>> ParsePlaylists(string path, CancellationToken? token = null) =>
        PlaylistsParser.ParseDocument(await OpenDocument(path, token ?? CancellationToken.None));

    private async Task<XDocument> OpenDocument(string path, CancellationToken token)
    {
        using var stream = File.OpenText(path);

        return await XDocument.LoadAsync(stream, LoadOptions.None, token);
    }
}
