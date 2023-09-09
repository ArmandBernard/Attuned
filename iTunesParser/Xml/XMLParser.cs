using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public class XmlParser: IXmlParser
{
    private readonly string _path;
    private ITrackListParser TrackListParser { get; }
    private IPlaylistsParser PlaylistsParser { get; }

    public XmlParser(ITrackListParser trackListParser, IPlaylistsParser playlistsParser, string path)
    {
        _path = path;
        TrackListParser = trackListParser;
        PlaylistsParser = playlistsParser;
    }

    public async Task<IEnumerable<Track>> ParseTracks(CancellationToken? token = null) =>
        TrackListParser.ParseDocument(await OpenDocument(_path, token ?? CancellationToken.None));

    public async Task<IEnumerable<Playlist>> ParsePlaylists(CancellationToken? token = null) =>
        PlaylistsParser.ParseDocument(await OpenDocument(_path, token ?? CancellationToken.None));

    private async Task<XDocument> OpenDocument(string path, CancellationToken token)
    {
        using var stream = File.OpenText(path);

        return await XDocument.LoadAsync(stream, LoadOptions.None, token);
    }
}
