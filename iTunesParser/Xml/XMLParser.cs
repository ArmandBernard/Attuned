using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public class XmlParser(ITrackListParser trackListParser, IPlaylistParser playlistParser, string path)
    : IXmlParser
{
    public async Task<IEnumerable<Track>> ParseTracks(CancellationToken? token = null) =>
        trackListParser.ParseDocument(await OpenDocument(path, token ?? CancellationToken.None));

    public async Task<IEnumerable<Playlist>> ParsePlaylists(CancellationToken? token = null) =>
        playlistParser.ParseDocument(await OpenDocument(path, token ?? CancellationToken.None));

    private static async Task<XDocument> OpenDocument(string path, CancellationToken token)
    {
        using var stream = File.OpenText(path);

        return await XDocument.LoadAsync(stream, LoadOptions.None, token);
    }
}
