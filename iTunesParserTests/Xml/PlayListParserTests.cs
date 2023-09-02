using System.Xml.Linq;
using FluentAssertions;
using iTunesSmartParser.Data;
using iTunesSmartParser.Xml;

namespace iTunesParserTests.Xml;

[TestFixture]
public class PlayListParserTests
{
    private readonly PlaylistParser _playlistParser = new();

    // lang=XML
    private const string TestDoc =
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
                        <dict>
                            <key>Track ID</key><integer>3716</integer>
                        </dict>
                        <dict>
                            <key>Track ID</key><integer>3712</integer>
                        </dict>
                        <dict>
                            <key>Track ID</key><integer>3698</integer>
                        </dict>
                        <dict>
                            <key>Track ID</key><integer>3704</integer>
                        </dict>
                    </array>
                </dict>
            </array>
        </dict>
        </plist>
        """;


    private readonly IEnumerable<Playlist> _expectedPlaylists = new[]
    {
        new Playlist(
            "5 Stars+",
            16371,
            new[]
            {
                3702,
                3694,
                3696,
                3716,
                3712,
                3698,
                3704
            },
            true,
            "( ([MediaKind] = 'Music') OR ([MediaKind] = 'Music Video') )\n\tAND ( ([Rating] >= 2 AND [Rating] <= 5) )")
    };

    [Test]
    public void PlayListParser_GivenValidDocument_ShouldReturnExpectedTrackList()
    {
        var doc = XDocument.Parse(TestDoc);

        var result = _playlistParser.ParsePlaylists(doc);

        result.Should().BeEquivalentTo(_expectedPlaylists);
    }
}