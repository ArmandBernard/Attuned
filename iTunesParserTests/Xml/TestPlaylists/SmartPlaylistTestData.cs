using System.Buffers.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using iTunesSmartParser.Xml;

namespace iTunesParserTests.Xml.TestPlaylists;

/// <summary>
/// Playlists saved by iTunes, to compare what is written to what iTunes writes
/// </summary>
public static partial class SmartPlaylistTestData
{
    // lang=XML
    public const string FIVE_STARS_PLUS =
        """
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
            </array>
        </dict>
        """;

    // lang=XML
    public const string REGULAR_PLAYLIST =
        """
        <dict>
            <key>Playlist ID</key><integer>5</integer>
            <key>Playlist Persistent ID</key><string>0000000000000005</string>
            <key>All Items</key><true/>
            <key>Name</key><string>Regular</string>
            <key>Playlist Items</key>
            <array>
                <dict>
                    <key>Track ID</key><integer>1</integer>
                </dict>
            </array>
        </dict>
        """;

    /// <summary>
    /// The smart playlists, as the XML iTunes writes for them.
    /// </summary>
    public static IEnumerable<TestCaseData> SmartPlaylists()
    {
        yield return new TestCaseData(FIVE_STARS_PLUS).SetArgDisplayNames("5 Stars+");
        yield return new TestCaseData(Load("BestOfWaveshaper")).SetArgDisplayNames("Best of Waveshaper");
        yield return new TestCaseData(Load("FavouriteClassical")).SetArgDisplayNames("favourite classical");
        // does not use its rules
        yield return new TestCaseData(Load("MostPlayed")).SetArgDisplayNames("Most played");

        // Made in iTunes to have each of these rules. These ones were not made by the same person who uses iTunes,
        // so "fresh" was made with nothing changed from what iTunes starts a new playlist with.
        yield return new TestCaseData(Load("TestFreshPlaylist")).SetArgDisplayNames("fresh playlist");
        yield return new TestCaseData(Load("TestDates")).SetArgDisplayNames("dates");
        yield return new TestCaseData(Load("TestRatings")).SetArgDisplayNames("ratings");
        yield return new TestCaseData(Load("TestStructure")).SetArgDisplayNames("structure");
    }

    public static string Load(string name) =>
        DeclarationRegex().Replace(File.ReadAllText(Path.Combine("Xml", "TestPlaylists", name + ".xml")), "");

    /// <summary>
    /// A library with the given playlists in it
    /// </summary>
    public static XDocument Library(params string[] playlistElements) => XDocument.Parse(
        "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
        "<plist version=\"1.0\">\n<dict>\n<key>Playlists</key>\n<array>\n" +
        string.Join("\n", playlistElements) +
        "\n</array>\n</dict>\n</plist>", LoadOptions.PreserveWhitespace);

    public static (byte[] Info, byte[] Criteria) Blobs(string playlistXml)
    {
        var dictionary = PListParser.ParseDictionary(XElement.Parse(playlistXml));

        return ((byte[]) dictionary["Smart Info"], (byte[]) dictionary["Smart Criteria"]);
    }

    #region building criteria by hand
    // These make criteria without using the encoder, to show what its bytes are meant to be.

    public static byte[] ListOfRules(int ruleCount, bool any, params byte[][] rules)
    {
        var header = new byte[136];
        "SLst"u8.CopyTo(header);
        header[5] = 1;
        header[7] = 1;
        BinaryPrimitives.WriteInt32BigEndian(header.AsSpan(8), ruleCount);
        header[15] = any ? (byte) 1 : (byte) 0;

        return header.Concat(rules.SelectMany(x => x)).ToArray();
    }

    /// <summary>
    /// A rule that is about a string
    /// </summary>
    public static byte[] StringRule(byte field, byte sign, byte @operator, string value)
    {
        var text = Encoding.BigEndianUnicode.GetBytes(value);
        var rule = new byte[56 + text.Length];
        rule[3] = field;
        rule[4] = sign;
        rule[7] = @operator;
        BinaryPrimitives.WriteInt32BigEndian(rule.AsSpan(52), text.Length);
        text.CopyTo(rule, 56);
        return rule;
    }

    #endregion

    [GeneratedRegex(@"<\?xml[^>]*\?>\s*")]
    private static partial Regex DeclarationRegex();
}
