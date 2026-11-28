using System.Text.RegularExpressions;
using System.Xml.Linq;
using iTunesParserTests.Xml.TestPlaylists;
using iTunesSmartParser.Data;
using iTunesSmartParser.Data.Limits;
using iTunesSmartParser.Data.Logic;
using iTunesSmartParser.Data.Rules;
using iTunesSmartParser.Fields;
using iTunesSmartParser.Playlists;
using iTunesSmartParser.Xml;

namespace iTunesParserTests.Xml;

[TestFixture]
public class PlaylistWriterTests
{
    // 16371 is "5 Stars+", 22432 is "Best of Waveshaper", 22804 is "Most played" (which has its rules turned off)
    // and 5 is a playlist that is not smart.
    private const int FIVE_STARS = 16371;
    private const int WAVESHAPER = 22432;
    private const int MOST_PLAYED = 22804;
    private const int REGULAR = 5;

    private static readonly string _bestOfWaveshaper = SmartPlaylistTestData.Load("BestOfWaveshaper");
    private static readonly string _mostPlayed = SmartPlaylistTestData.Load("MostPlayed");

    private readonly IPlaylistParser _parser = new PlaylistParser();
    private PlaylistWriter _writer = null!;
    private XDocument _doc = null!;

    private static XDocument NewDocument() => SmartPlaylistTestData.Library(
        SmartPlaylistTestData.FIVE_STARS_PLUS, _bestOfWaveshaper, _mostPlayed, SmartPlaylistTestData.REGULAR_PLAYLIST);

    [SetUp]
    public void SetUp()
    {
        _writer = new PlaylistWriter(_parser);
        _doc = NewDocument();
    }

    private PlaylistDetails Details(int id) => _parser.GetDetailsById(_doc, id)!;

    private XElement PlaylistElement(int id) => _doc.Descendants("dict")
        .First(x => x.PlistDictGet("Playlist ID")?.Value == id.ToString());

    private byte[] Blob(int id, string key) => (byte[]) PlaylistElement(id).PlistDictGet(key)!.PlistParseValue();

    private void AssertDocumentUnchanged() => XNode.DeepEquals(_doc, NewDocument()).Should().BeTrue();

    private static readonly Conjunction _newRules = new(ConjunctionType.Or, new List<Conjunction>
    {
        new(ConjunctionType.And, [], new List<IRule>
        {
            new StringRule(StringFields.Genre, Operator.Is, Sign.StringPositive, "Jazz"),
            new IntRule(IntFields.Year, Operator.Greater, Sign.IntPositive, 1990, null)
        })
    }, new List<IRule>
    {
        new TimeSpanRule(DateFields.DateAdded, Operator.Other, Sign.IntPositive, TimeSpan.FromDays(14))
    });

    #region general

    [Test]
    public void ApplyPatch_UnknownPlaylist_ReturnsNullAndLeavesDocumentUnchanged()
    {
        var result = _writer.ApplyPatch(_doc, 99, new PlaylistPatch { Name = "New" });

        result.Should().BeNull();
        AssertDocumentUnchanged();
    }

    [Test]
    public void ApplyPatch_EmptyPatch_LeavesDocumentUnchanged()
    {
        _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch());

        AssertDocumentUnchanged();
    }

    [Test]
    public void ApplyPatch_ReturnsTheUpdatedPlaylist()
    {
        var result = _writer.ApplyPatch(_doc, WAVESHAPER,
            new PlaylistPatch { Name = "Renamed", LiveUpdating = false });

        result.Should().BeEquivalentTo(Details(WAVESHAPER), options => options.IncludingAllRuntimeProperties());
        result!.Name.Should().Be("Renamed");
        result.Filters!.LiveUpdating.Should().BeFalse();
    }

    [Test]
    public void ApplyPatch_DoesNotChangeOtherPlaylists()
    {
        var before = new[] {FIVE_STARS, MOST_PLAYED, REGULAR}.Select(x => PlaylistElement(x).ToString()).ToList();

        _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch
        {
            Name = "Renamed",
            LiveUpdating = false,
            Limit = new Limit(true, LimitUnits.GB, 4, true, SelectionMethods.Genre, false),
            Rules = _newRules
        });

        new[] {FIVE_STARS, MOST_PLAYED, REGULAR}.Select(x => PlaylistElement(x).ToString()).Should().Equal(before);
    }

    [Test]
    public void ApplyPatch_DoesNotChangeTheTracksInThePlaylist()
    {
        var before = Details(WAVESHAPER).Items.ToList();

        _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch { Rules = _newRules });

        Details(WAVESHAPER).Items.Should().Equal(before);
    }

    #endregion

    #region name

    [Test]
    public void ApplyPatch_Name_OnlyChangesTheName()
    {
        var before = Details(WAVESHAPER);

        _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch { Name = "New name" });

        Details(WAVESHAPER).Should().BeEquivalentTo(before with { Name = "New name" },
            options => options.IncludingAllRuntimeProperties());
    }

    [Test]
    public void ApplyPatch_Name_WorksOnAPlaylistThatIsNotSmart()
    {
        var result = _writer.ApplyPatch(_doc, REGULAR, new PlaylistPatch { Name = "Not smart" });

        result!.Name.Should().Be("Not smart");
        result.IsSmart.Should().BeFalse();
    }

    [Test]
    public void ApplyPatch_Name_WithSpecialCharactersRoundTrips()
    {
        const string name = "<Fish & \"Chips\"> 日本語";

        _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch { Name = name });

        var roundTripped = XDocument.Parse(_doc.ToString());
        _parser.GetById(roundTripped, WAVESHAPER)!.Name.Should().Be(name);
    }

    #endregion

    #region limit and live updating

    [Test]
    public void ApplyPatch_Limit_OnlyChangesTheLimit()
    {
        var before = Details(WAVESHAPER);
        var limit = new Limit(true, LimitUnits.Hours, 3, true, SelectionMethods.RecentlyPlayed, false);

        _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch { Limit = limit });

        var after = Details(WAVESHAPER);
        after.Filters!.Limit.Should().Be(limit);
        after.Should().BeEquivalentTo(before with { Filters = before.Filters! with { Limit = limit } },
            options => options.IncludingAllRuntimeProperties());
    }

    [Test]
    public void ApplyPatch_LiveUpdating_OnlyChangesLiveUpdating()
    {
        var before = Details(WAVESHAPER);

        _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch { LiveUpdating = false });

        Details(WAVESHAPER).Should().BeEquivalentTo(before with { Filters = before.Filters! with { LiveUpdating = false } },
            options => options.IncludingAllRuntimeProperties());
    }

    [Test]
    public void ApplyPatch_Limit_KeepsTheBytesOfTheInfoThatAreNotUnderstood()
    {
        // iTunes set a byte of this playlist's info that nothing is known about.
        Blob(FIVE_STARS, "Smart Info")[21].Should().Be(1);

        _writer.ApplyPatch(_doc, FIVE_STARS, new PlaylistPatch
        {
            Limit = new Limit(true, LimitUnits.Items, 10, false, SelectionMethods.Random, true), LiveUpdating = false
        });

        var info = Blob(FIVE_STARS, "Smart Info");
        info[21].Should().Be(1);
        info[19].Should().Be(7);
    }

    [Test]
    public void ApplyPatch_Limit_DoesNotChangeTheCriteria()
    {
        var before = PlaylistElement(WAVESHAPER).PlistDictGet("Smart Criteria")!.Value;

        _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch
        {
            Limit = new Limit(true, LimitUnits.Items, 10, false, SelectionMethods.Random, true)
        });

        PlaylistElement(WAVESHAPER).PlistDictGet("Smart Criteria")!.Value.Should().Be(before);
    }

    #endregion

    #region rules

    [Test]
    public void ApplyPatch_Rules_ReplacesTheRules()
    {
        var result = _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch { Rules = _newRules });

        result!.Filters!.RuleConjunction.Should().BeEquivalentTo(_newRules, options => options.IncludingAllRuntimeProperties());
        Details(WAVESHAPER).Filters!.RuleConjunction.Should()
            .BeEquivalentTo(_newRules, options => options.IncludingAllRuntimeProperties());
    }

    [Test]
    public void ApplyPatch_Rules_OnlyChangesTheRules()
    {
        var before = Details(WAVESHAPER);

        _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch { Rules = _newRules });

        Details(WAVESHAPER).Should().BeEquivalentTo(before with { Filters = before.Filters! with { RuleConjunction = _newRules } },
            options => options.IncludingAllRuntimeProperties());
    }

    [Test]
    public void ApplyPatch_Rules_StoresTheBytesTheEncoderMakes()
    {
        _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch { Rules = _newRules });

        Blob(WAVESHAPER, "Smart Criteria").Should().Equal(
            CriteriaEncoder.Encode(_newRules, SmartPlaylistTestData.Blobs(_bestOfWaveshaper).Criteria));
    }

    [Test]
    public void ApplyPatch_Rules_TurnsOnUsingTheRules()
    {
        Details(MOST_PLAYED).Filters!.RuleConjunction.Should().BeNull();

        _writer.ApplyPatch(_doc, MOST_PLAYED, new PlaylistPatch { Rules = _newRules });

        Details(MOST_PLAYED).Filters!.RuleConjunction.Should()
            .BeEquivalentTo(_newRules, options => options.IncludingAllRuntimeProperties());
    }

    [Test]
    public void ApplyPatch_NullRules_TurnsOffUsingTheRulesAndKeepsThem()
    {
        var criteriaBefore = PlaylistElement(WAVESHAPER).PlistDictGet("Smart Criteria")!.Value;
        var rulesBefore = Details(WAVESHAPER).Filters!.RuleConjunction;

        _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch { Rules = Option<Conjunction?>.Of(null) });

        Details(WAVESHAPER).Filters!.RuleConjunction.Should().BeNull();
        PlaylistElement(WAVESHAPER).PlistDictGet("Smart Criteria")!.Value.Should().Be(criteriaBefore);

        // and they are there when it is turned back on
        _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch { Rules = rulesBefore });
        Details(WAVESHAPER).Filters!.RuleConjunction.Should()
            .BeEquivalentTo(rulesBefore, options => options.IncludingAllRuntimeProperties());
    }

    [Test]
    public void ApplyPatch_Rules_KeepsTheOrderOfRulesAndGroupsInThePlaylist()
    {
        var (info, _) = SmartPlaylistTestData.Blobs(SmartPlaylistTestData.FIVE_STARS_PLUS);
        var group = new Conjunction(ConjunctionType.Or, [],
            [new StringRule(StringFields.Genre, Operator.Is, Sign.StringPositive, "Jazz")]);
        var rule = new IntRule(IntFields.Year, Operator.Is, Sign.IntPositive, 2000, null);

        // a group before a rule, which isn't how the encoder writes new rules
        var groupThenRule = SmartPlaylistTestData.ListOfRules(2, false,
            CriteriaEncoder.Encode(new Conjunction(ConjunctionType.And, [group], [])).Skip(136).ToArray(),
            CriteriaEncoder.Encode(new Conjunction(ConjunctionType.And, [], [rule])).Skip(136).ToArray());

        var doc = SmartPlaylistTestData.Library($"""
            <dict>
                <key>Playlist ID</key><integer>77</integer>
                <key>Name</key><string>Group first</string>
                <key>Smart Info</key><data>{Convert.ToBase64String(info)}</data>
                <key>Smart Criteria</key><data>{Convert.ToBase64String(groupThenRule)}</data>
            </dict>
            """);
        var edited = _parser.GetDetailsById(doc, 77)!.Filters!.RuleConjunction!;

        _writer.ApplyPatch(doc, 77, new PlaylistPatch { Rules = edited });

        var stored = (byte[]) doc.Descendants("dict").First(x => x.PlistDictGet("Playlist ID")?.Value == "77")
            .PlistDictGet("Smart Criteria")!.PlistParseValue();
        stored.Should().Equal(groupThenRule);
    }

    #endregion

    #region not changing things that don't change

    [TestCaseSource(typeof(SmartPlaylistTestData), nameof(SmartPlaylistTestData.SmartPlaylists))]
    public void ApplyPatch_GivenTheValuesThePlaylistHas_LeavesTheDocumentExactlyAsItWas(string playlistXml)
    {
        var doc = SmartPlaylistTestData.Library(playlistXml);
        var id = int.Parse(PListParser.ParseDictionary(XElement.Parse(playlistXml))["Playlist ID"].ToString());
        var current = _parser.GetDetailsById(doc, id)!;

        _writer.ApplyPatch(doc, id, new PlaylistPatch
        {
            Name = current.Name,
            Limit = current.Filters!.Limit,
            LiveUpdating = current.Filters.LiveUpdating,
            Rules = current.Filters.RuleConjunction
        });

        XNode.DeepEquals(doc, SmartPlaylistTestData.Library(playlistXml)).Should().BeTrue();
    }

    #endregion

    #region layout

    [Test]
    public void ApplyPatch_Rules_WritesDataInTheSameLayoutAsITunes()
    {
        var original = PlaylistElement(WAVESHAPER).PlistDictGet("Smart Criteria")!.Value;
        var originalLines = original.Trim().Split('\n').Select(x => x.Trim()).ToList();

        _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch { Rules = _newRules });

        var text = PlaylistElement(WAVESHAPER).PlistDictGet("Smart Criteria")!.Value;
        var lines = text.Trim().Split('\n').Select(x => x.Trim()).ToList();

        // the same line length, with a shorter last line
        lines.Take(lines.Count - 1).Should().OnlyContain(x => x.Length == originalLines[0].Length);
        lines.Last().Length.Should().BeLessThanOrEqualTo(originalLines[0].Length);
        // the same space around the lines
        Regex.Match(text, @"^\s*").Value.Should().Be(Regex.Match(original, @"^\s*").Value);
        Regex.Match(text, @"\s*$").Value.Should().Be(Regex.Match(original, @"\s*$").Value);
        text.Should().MatchRegex(@"^(\s+[A-Za-z0-9+/=]+)+\s+$");
    }

    [Test]
    public void ApplyPatch_Rules_KeepsLineBreaksAndTabsOfALibraryThatITunesWrote()
    {
        // iTunes on Windows uses tabs for indentation and CRLF for line breaks
        var criteria = Regex.Replace(PlaylistElement(WAVESHAPER).PlistDictGet("Smart Criteria")!.Value, @"\n\s*", "\r\n\t\t\t");
        PlaylistElement(WAVESHAPER).PlistDictGet("Smart Criteria")!.Value = criteria;

        _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch { Rules = _newRules });

        var text = PlaylistElement(WAVESHAPER).PlistDictGet("Smart Criteria")!.Value;
        text.Should().StartWith("\r\n\t\t\t").And.EndWith("\r\n\t\t\t");
        text.Replace("\r\n\t\t\t", "").Should().MatchRegex("^[A-Za-z0-9+/=]+$");
        text.Should().NotContain("\n ", "there is no indentation that is not tabs");
    }

    #endregion

    #region things that can't be done

    [Test]
    public void ApplyPatch_SmartInformationOnAPlaylistThatIsNotSmart_ThrowsAndLeavesDocumentUnchanged()
    {
        var act = () => _writer.ApplyPatch(_doc, REGULAR, new PlaylistPatch
        {
            Name = "Renamed", LiveUpdating = true
        });

        act.Should().Throw<InvalidOperationException>().WithMessage("*smart playlist*");
        AssertDocumentUnchanged();
    }

    [Test]
    public void ApplyPatch_ARuleThatCantBeStored_ThrowsAndLeavesDocumentUnchanged()
    {
        var badRules = new Conjunction(ConjunctionType.And, [], new List<IRule>
        {
            new StringRule(StringFields.Artist, Operator.Greater, Sign.StringPositive, "x")
        });

        var act = () => _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch
        {
            Name = "Renamed",
            Limit = new Limit(true, LimitUnits.GB, 4, true, SelectionMethods.Genre, false),
            Rules = badRules
        });

        act.Should().Throw<ArgumentException>();
        AssertDocumentUnchanged();
    }

    [Test]
    public void ApplyPatch_ALimitThatCantBeStored_ThrowsAndLeavesDocumentUnchanged()
    {
        var act = () => _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch
        {
            Name = "Renamed", Limit = new Limit(true, LimitUnits.Items, -5, false, SelectionMethods.Random, true)
        });

        act.Should().Throw<ArgumentOutOfRangeException>();
        AssertDocumentUnchanged();
    }

    [Test]
    public void ApplyPatch_ARuleAboutAPlaylist_ThrowsAndLeavesDocumentUnchanged()
    {
        var act = () => _writer.ApplyPatch(_doc, WAVESHAPER, new PlaylistPatch
        {
            Rules = new Conjunction(ConjunctionType.And, [], new List<IRule>
            {
                new PlaylistRule(PlaylistFields.PlaylistPersistentId, Operator.Is, Sign.IntPositive, "0AB4B9146BF18882")
            })
        });

        act.Should().Throw<NotSupportedException>();
        AssertDocumentUnchanged();
    }

    #endregion
}
