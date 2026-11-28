using iTunesParserTests.Xml.TestPlaylists;
using iTunesSmartParser.Data;
using iTunesSmartParser.Data.Limits;
using iTunesSmartParser.Playlists;

namespace iTunesParserTests.Playlists;

[TestFixture]
public class InfoWriterTests
{
    // a byte that no known setting is at, filled with something that isn't what any setting would write
    private const byte UNKNOWN = 0xAB;

    private static readonly int[] _limitOffsets = [2, 3, 7, 8, 9, 10, 11, 12, 13];

    private static byte[] InfoOf(string playlistXml) => SmartPlaylistTestData.Blobs(playlistXml).Info;

    private static SmartPlaylistInformation ParseInfo(byte[] info)
    {
        var helper = new InfoHelper(info);

        return new SmartPlaylistInformation(
            new Limit(helper.IsLimited, helper.LimitUnits, helper.LimitNumber, helper.OnlyChecked, helper.SortBy,
                helper.SortDescending),
            helper.Match ? new Conjunction(ConjunctionType.And, [], []) : null,
            helper.LiveUpdate);
    }

    private static byte[] FilledWithUnknown()
    {
        var info = new byte[InfoWriter.LENGTH];
        Array.Fill(info, UNKNOWN);
        return info;
    }

    private static IEnumerable<int> ChangedOffsets(byte[] before, byte[] after) =>
        Enumerable.Range(0, before.Length).Where(i => before[i] != after[i]);

    #region matching what iTunes writes

    [TestCaseSource(typeof(SmartPlaylistTestData), nameof(SmartPlaylistTestData.SmartPlaylists))]
    public void Encode_GivenTheInfoThatITunesWrote_GivesTheSameBytes(string playlistXml)
    {
        var info = InfoOf(playlistXml);

        InfoWriter.Encode(info, ParseInfo(info)).Should().Equal(info);
    }

    [TestCaseSource(typeof(SmartPlaylistTestData), nameof(SmartPlaylistTestData.SmartPlaylists))]
    public void Encode_ForANewPlaylist_IsTheSameAsITunesExceptForOneByteNothingIsKnownAbout(string playlistXml)
    {
        var info = InfoOf(playlistXml);

        var result = InfoWriter.Encode(ParseInfo(info));

        // iTunes sets byte 21 in about a third of playlists. See InfoWriter.
        ChangedOffsets(info, result).Should().BeSubsetOf([21]);
    }

    [Test]
    public void Encode_ForANewPlaylist_HasTheBytesITunesGives()
    {
        // The playlist "Most played" in iTunes: live, no rules, limited to 50 items by most often played
        var expected = InfoOf(SmartPlaylistTestData.Load("MostPlayed"));

        var result = InfoWriter.Encode(new SmartPlaylistInformation(
            new Limit(true, LimitUnits.Items, 50, false, SelectionMethods.OftenPlayed, true), null, true));

        result.Should().Equal(expected);
    }

    #endregion

    #region changing known values

    [Test]
    public void SetLimit_OnlyChangesTheBytesAboutTheLimit()
    {
        var original = FilledWithUnknown();

        var result = InfoWriter.SetLimit(original,
            new Limit(true, LimitUnits.GB, 7, true, SelectionMethods.RecentlyAdded, false));

        ChangedOffsets(original, result).Should().BeSubsetOf(_limitOffsets);
    }

    [Test]
    public void SetLimit_IsReadBackTheSame()
    {
        var limit = new Limit(true, LimitUnits.Hours, 2000, true, SelectionMethods.LowestRating, false);

        var result = InfoWriter.SetLimit(InfoWriter.NewInfo(), limit);

        ParseInfo(result).Limit.Should().Be(limit);
    }

    [Test]
    public void SetLimit_StoresTheAmountAsFourBigEndianBytes() =>
        InfoWriter.SetLimit(InfoWriter.NewInfo(), new Limit(true, LimitUnits.Items, 2000, false, SelectionMethods.Random, true))
            .Skip(8).Take(4).Should().Equal(0, 0, 0x07, 0xD0);

    [TestCase(LimitUnits.Minutes, 1)]
    [TestCase(LimitUnits.MB, 2)]
    [TestCase(LimitUnits.Items, 3)]
    [TestCase(LimitUnits.Hours, 4)]
    [TestCase(LimitUnits.GB, 5)]
    public void SetLimit_StoresTheUnits(LimitUnits units, byte expected) =>
        InfoWriter.SetLimit(InfoWriter.NewInfo(), new Limit(true, units, 1, false, SelectionMethods.Random, true))[3]
            .Should().Be(expected);

    [TestCase(true, 0)]
    [TestCase(false, 1)]
    public void SetLimit_StoresMostAsZeroAndLeastAsOne(bool descending, byte expected) =>
        InfoWriter.SetLimit(InfoWriter.NewInfo(), new Limit(true, LimitUnits.Items, 1, false, SelectionMethods.Name, descending))[13]
            .Should().Be(expected);

    [Test]
    public void SetLimit_GivenAnUnknownUnit_Throws()
    {
        var act = () => InfoWriter.SetLimit(InfoWriter.NewInfo(),
            new Limit(true, (LimitUnits) 9, 1, false, SelectionMethods.Random, true));

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void SetLimit_GivenAnUnknownSortField_Throws()
    {
        var act = () => InfoWriter.SetLimit(InfoWriter.NewInfo(),
            new Limit(true, LimitUnits.Items, 1, false, (SelectionMethods) 0x7f, true));

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void SetLimit_GivenANegativeAmount_Throws()
    {
        var act = () => InfoWriter.SetLimit(InfoWriter.NewInfo(),
            new Limit(true, LimitUnits.Items, -1, false, SelectionMethods.Random, true));

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestCase(true)]
    [TestCase(false)]
    public void SetLiveUpdating_OnlyChangesTheLiveUpdatingByte(bool live)
    {
        var original = FilledWithUnknown();

        var result = InfoWriter.SetLiveUpdating(original, live);

        ChangedOffsets(original, result).Should().BeSubsetOf([0]);
        result[0].Should().Be(live ? (byte) 1 : (byte) 0);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void SetMatching_OnlyChangesTheMatchingByte(bool matching)
    {
        var original = FilledWithUnknown();

        var result = InfoWriter.SetMatching(original, matching);

        ChangedOffsets(original, result).Should().BeSubsetOf([1]);
        result[1].Should().Be(matching ? (byte) 1 : (byte) 0);
    }

    [Test]
    public void Encode_KeepsEveryByteThatIsNotUnderstood()
    {
        var original = FilledWithUnknown();

        var result = InfoWriter.Encode(original, new SmartPlaylistInformation(
            new Limit(true, LimitUnits.Items, 25, true, SelectionMethods.Genre, false),
            new Conjunction(ConjunctionType.Or, [], []), true));

        ChangedOffsets(original, result).Should().BeSubsetOf([0, 1, .. _limitOffsets]);
        result[19].Should().Be(UNKNOWN);
        result[21].Should().Be(UNKNOWN);
    }

    [Test]
    public void Setters_DoNotChangeTheInfoTheyAreGiven()
    {
        var original = InfoWriter.NewInfo();
        var copy = (byte[]) original.Clone();

        InfoWriter.SetLimit(original, new Limit(true, LimitUnits.GB, 5, true, SelectionMethods.Album, false));
        InfoWriter.SetLiveUpdating(original, true);
        InfoWriter.SetMatching(original, true);

        original.Should().Equal(copy);
    }

    [Test]
    public void SetLiveUpdating_GivenInfoOfTheWrongLength_Throws()
    {
        var act = () => InfoWriter.SetLiveUpdating(new byte[10], true);

        act.Should().Throw<ArgumentException>().WithMessage("*112 bytes*");
    }

    #endregion

    [Test]
    public void NewInfo_IsAllZerosExceptForTheSevenITunesAlwaysWrites()
    {
        var info = InfoWriter.NewInfo();

        info.Should().HaveCount(112);
        info.Select((value, index) => (value, index)).Where(x => x.value != 0).Should()
            .BeEquivalentTo([(value: (byte) 7, index: 19)]);
    }
}
