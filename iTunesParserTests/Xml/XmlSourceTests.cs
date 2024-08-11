using iTunesSmartParser.Xml;

namespace iTunesParserTests.Xml;

[TestFixture]
public class XmlSourceTests
{
    private const string PATH = "iTunesMusicLibrary.xml";

    private readonly XmlSource _xmlSource = new(PATH);

    private readonly ITrackListParser _trackListParser = new TrackListParser();

    private readonly IPlaylistParser _playlistParser = new PlaylistParser();

    [Test]
    [Explicit]
    [Category("LocalOnly")]
    // This test can only be run locally as a library is private
    public async Task ParseTracks_WorksWithARealLibrary()
    {
        var doc = await _xmlSource.GetXDocument(CancellationToken.None);

        var tracks = _trackListParser.ParseDocument(doc);

        tracks.Should().HaveCountGreaterThan(1);
    }

    [Test]
    [Explicit]
    [Category("LocalOnly")]
    // This test can only be run locally as a library is private
    public async Task ParsePlaylists_WorksWithARealLibrary()
    {
        var doc = await _xmlSource.GetXDocument(CancellationToken.None);

        var playlists = _playlistParser.ParseDocument(doc);

        playlists.Should().HaveCountGreaterThan(1);
    }
}