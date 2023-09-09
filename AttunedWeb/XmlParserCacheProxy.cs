using iTunesSmartParser.Data;
using iTunesSmartParser.Xml;

namespace AttunedWebApi;

public class XmlParserCacheProxy : IXmlParser
{
    private readonly XmlParser _xmlParser;

    private IReadOnlyList<Track>? TrackCache { get; set; }
    private IReadOnlyList<Playlist>? PlaylistCache { get; set; }

    public XmlParserCacheProxy(XmlParser xmlParser)
    {
        _xmlParser = xmlParser;
    }

    public async void InvalidateTrackCache()
    {
        TrackCache = (await _xmlParser.ParseTracks(CancellationToken.None)).ToArray();
    }

    public async Task<IEnumerable<Track>> ParseTracks(CancellationToken? token = null)
    {
        return TrackCache ??= (await _xmlParser.ParseTracks(token)).ToArray();
    }

    public async void InvalidatePlaylistCache()
    {
        PlaylistCache = (await _xmlParser.ParsePlaylists(CancellationToken.None)).ToArray();
    }

    public async Task<IEnumerable<Playlist>> ParsePlaylists(CancellationToken? token = null)
    {
        return PlaylistCache ??= (await _xmlParser.ParsePlaylists(token)).ToArray();
    }
}