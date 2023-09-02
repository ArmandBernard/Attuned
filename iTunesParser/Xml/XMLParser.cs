using System.Xml.Linq;

namespace iTunesSmartParser.Xml;

public class XMLParser
{
    private ITrackListParser TrackListParser { get; init; }
    public IPlaylistsParser PlaylistsParser { get; }

    public XMLParser(ILogger logger, ITrackListParser trackListParser, IPlaylistsParser playlistsParser)
    {
        Logger = logger;
        TrackListParser = trackListParser;
        PlaylistsParser = playlistsParser;
    }

    private ILogger Logger { get; init; }

    /// <summary>
    /// Load all tracks from the iTunes library XML using the XDoc method
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Track>> LoadTracks(string path, CancellationToken? token = null) =>
        TrackListParser.ParseTracks(await OpenDocument(path, token ?? CancellationToken.None));

    public async Task<IEnumerable<Playlist>> LoadSmartPlaylists(string path, CancellationToken? token = null) =>
        PlaylistsParser.ParsePlaylists(await OpenDocument(path, token ?? CancellationToken.None));

    private async Task<XDocument> OpenDocument(string path, CancellationToken token)
    {
        using var stream = File.OpenText(path);

        return await XDocument.LoadAsync(stream, LoadOptions.None, token);
    }
}
