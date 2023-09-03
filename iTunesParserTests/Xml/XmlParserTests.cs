using iTunesSmartParser.Xml;

namespace iTunesParserTests.Xml;

[TestFixture]
public class XmlParserTests
{
    private readonly XmlParser _xmlParser = new(new TrackListParser(), new PlaylistParser());

    [Test]
    [Explicit]
    [Category("LocalOnly")]
    // This test can only be run locally as a library is private
    public void ParseTracks_WorksWithARealLibrary()
    {
        var tracks = _xmlParser.ParseTracks("iTunesMusicLibrary.xml").Result;

        tracks.Should().HaveCountGreaterThan(1);
    }
    
    [Test]
    [Explicit]
    [Category("LocalOnly")]
    // This test can only be run locally as a library is private
    public void ParsePlaylists_WorksWithARealLibrary()
    {
        var playlists = _xmlParser.ParsePlaylists("iTunesMusicLibrary.xml").Result;

        playlists.Should().HaveCountGreaterThan(1);
    }
}