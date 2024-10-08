using System.Xml.Linq;
using iTunesParserTests.Xml.TestPlaylists;
using iTunesSmartParser.Data;
using iTunesSmartParser.Data.Limits;
using iTunesSmartParser.Data.Logic;
using iTunesSmartParser.Data.Rules;
using iTunesSmartParser.Fields;
using iTunesSmartParser.Xml;
using Newtonsoft.Json;

namespace iTunesParserTests.Xml;

[TestFixture]
public class PlayListParserTests
{
    private readonly IPlaylistParser _playlistParser = new PlaylistParser();

    // lang=XML
    private const string TEST_DOC =
        """
        <?xml version="1.0" encoding="UTF-8"?>
        <!DOCTYPE plist PUBLIC "-//Apple Computer//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
        <plist version="1.0">
        <dict>
            <key>Playlists</key>
            <array>
                <dict>
                    <key>Playlist ID</key><integer>16371</integer>
                    <key>Playlist Persistent ID</key><string>1468DB66452C9B7F</string>
                    <key>All Items</key><true/>
                    <key>Name</key><string>5 Stars+</string>
                    <key>Smart Info</key>
                    <data>
                    AQEAAwAAAAIAAAAZAAAAAAAAAAcAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
                    AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
                    AAAAAA==
                    </data>
                    <key>Smart Criteria</key>
                    <data>
                    U0xzdAABAAEAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
                    AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
                    AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAQAAAAAAAAAAAAAAAAAAAAAA
                    AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGAU0xzdAABAAEAAAACAAAAAQAAAAAAAAAA
                    AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
                    AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
                    AAAAAAAAADwAAAABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
                    AAAAAABEAAAAAAAAAAEAAAAAAAAAAAAAAAAAAAABAAAAAAAAAAEAAAAAAAAAAAAAAAAAAAAB
                    AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA8AAAAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
                    AAAAAAAAAAAAAAAAAAAAAAAAAAAARAAAAAAAAAAgAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAg
                    AAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEBAAAAAAAAAAAA
                    AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQRTTHN0AAEAAQAAAAEAAAAA
                    AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
                    AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
                    AAAAAAAAAAAAAAAAAAAAGQAAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
                    AAAAAAAAAAAAAAAAAEQAAAAAAAAAKAAAAAAAAAAAAAAAAAAAAAEAAAAAAAAAbQAAAAAAAAAA
                    AAAAAAAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAAA==
                    </data>
                    <key>Playlist Items</key>
                    <array>
                        <dict>
                            <key>Track ID</key><integer>3702</integer>
                        </dict>
                        <dict>
                            <key>Track ID</key><integer>3694</integer>
                        </dict>
                        <dict>
                            <key>Track ID</key><integer>3696</integer>
                        </dict>
                    </array>
                </dict>
            </array>
        </dict>
        </plist>
        """;


    private readonly IEnumerable<Playlist> _expectedPlaylists = new[]
    {
        new Playlist("5 Stars+", 16371, true)
    };

    private readonly PlaylistDetails _expectedPlaylistDetails = new PlaylistDetails(
        "5 Stars+",
        16371,
        true,
        new[]
        {
            3702,
            3694,
            3696,
        },
        //"( ([MediaKind] = 'Music') OR ([MediaKind] = 'Music Video') )\n\tAND ( ([Rating] >= 2 AND [Rating] <= 5) )")
        new SmartPlaylistInformation(
            new Limit(false, LimitUnits.Items, 25, false, SelectionMethods.Random, true),
            new Conjunction(ConjunctionType.And, new List<Conjunction>()
            {
                new(ConjunctionType.Or, Array.Empty<Conjunction>(), new List<IRule>()
                {
                    new DictionaryRule(DictionaryFields.MediaKind, Operator.Is, Sign.IntPositive, "Music"),
                    new DictionaryRule(DictionaryFields.MediaKind, Operator.Is, Sign.IntPositive,
                        "Music Video")
                }),
                new(ConjunctionType.And, Array.Empty<Conjunction>(), new List<IRule>()
                {
                    new IntRule(IntFields.Rating, Operator.Other, Sign.IntPositive, 2, 5)
                })
            }, Array.Empty<IRule>()),
            true
        ));

    [Test]
    public void ParseDocument_GivenValidDocument_ShouldReturnExpectedPlaylist()
    {
        var doc = XDocument.Parse(TEST_DOC);

        var result = _playlistParser.ParseDocument(doc);

        result.Should().BeEquivalentTo(_expectedPlaylists);
    }
    
    [Test]
    public void GetById_GivenValidDocumentAndId_ShouldReturnExpectedPlaylistDetails()
    {
        var doc = XDocument.Parse(TEST_DOC);

        var result = _playlistParser.GetById(doc, 16371);

        result.Should().BeEquivalentTo(_expectedPlaylistDetails);
    }
    
    [Test]
    public void GetById_GivenValidDocumentAndMissingId_ShouldReturnNull()
    {
        var doc = XDocument.Parse(TEST_DOC);

        var result = _playlistParser.GetById(doc, 2);

        result.Should().BeNull();
    }

    public static IEnumerable<TestCaseData> TestPlaylistsSource()
    {
        yield return new TestCaseData(File.ReadAllText(Path.Combine("Xml", "TestPlaylists", "BestOfWaveshaper.xml")),
            ExpectedPlaylistOutputs.BestOfWaveshaper);
        yield return new TestCaseData(File.ReadAllText(Path.Combine("Xml", "TestPlaylists", "FavouriteClassical.xml")),
            ExpectedPlaylistOutputs.FavouriteClassical);
        yield return new TestCaseData(File.ReadAllText(Path.Combine("Xml", "TestPlaylists", "MostPlayed.xml")),
            ExpectedPlaylistOutputs.MostPlayed);
    }

    [TestCaseSource(nameof(TestPlaylistsSource))]
    public void PlayListParser_GivenAVarietyOfSmartPlaylists_ParsesAllOfThemCorrectly(string xml,
        PlaylistDetails expectedOutput)
    {
        var parsedPlaylist = _playlistParser.ParsePlaylistDetails(XElement.Parse(xml));

        var converters = new JsonConverter[]
        {
            new Newtonsoft.Json.Converters.StringEnumConverter()
        };

        // serialization need to ensure recursive equality
        var parsedPlaylistJson = JsonConvert.SerializeObject(parsedPlaylist, Formatting.Indented, converters);
        var expectedOutputJson = JsonConvert.SerializeObject(expectedOutput, Formatting.Indented, converters);

        parsedPlaylistJson.Should().Be(expectedOutputJson);
    }
}