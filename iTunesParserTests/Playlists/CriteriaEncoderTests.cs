using iTunesParserTests.Xml.TestPlaylists;
using iTunesSmartParser.Data;
using iTunesSmartParser.Data.Logic;
using iTunesSmartParser.Data.Rules;
using iTunesSmartParser.Fields;
using iTunesSmartParser.Playlists;

namespace iTunesParserTests.Playlists;

[TestFixture]
public class CriteriaEncoderTests
{
    private static Conjunction And(IList<IRule> rules, params Conjunction[] groups) =>
        new(ConjunctionType.And, groups, rules);

    private static Conjunction Or(IList<IRule> rules, params Conjunction[] groups) =>
        new(ConjunctionType.Or, groups, rules);

    private static IList<IRule> Rules(params IRule[] rules) => rules;

    private static void AssertRoundTrips(Conjunction conjunction) =>
        CriteriaParser.Parse(CriteriaEncoder.Encode(conjunction)).Should()
            .BeEquivalentTo(conjunction, options => options.IncludingAllRuntimeProperties());

    #region matching what iTunes writes

    [TestCaseSource(typeof(SmartPlaylistTestData), nameof(SmartPlaylistTestData.SmartPlaylists))]
    public void Encode_GivenWhatTheParserReadsFromITunes_GivesTheSameBytes(string playlistXml)
    {
        var (_, criteria) = SmartPlaylistTestData.Blobs(playlistXml);

        var result = CriteriaEncoder.Encode(CriteriaParser.Parse(criteria));

        result.Should().Equal(criteria);
    }

    [Test]
    public void Encode_GivenAStringRule_HasTheBytesITunesGives()
    {
        // built by hand, from what was found by comparing the criteria of real playlists
        var expected = SmartPlaylistTestData.ListOfRules(1, false,
            SmartPlaylistTestData.StringRule(field: 0x04, sign: 1, @operator: 2, "Waveshaper"));

        var result = CriteriaEncoder.Encode(And(Rules(
            new StringRule(StringFields.Artist, Operator.Contains, Sign.StringPositive, "Waveshaper"))));

        result.Should().Equal(expected);
    }

    [Test]
    public void Encode_GivenNestedGroups_HasTheLengthsOfTheGroupsInThem()
    {
        var result = CriteriaEncoder.Encode(And(Rules(),
            Or(Rules(new StringRule(StringFields.Genre, Operator.Is, Sign.StringPositive, "Jazz")))));

        // header, then a group rule (56 bytes) which contains a list of its own (a header and a rule of 56 + 8 bytes)
        const int ruleLength = 56 + 2 * 4;
        result.Should().HaveCount(136 + 56 + 136 + ruleLength);
        // the group says how many bytes its list has
        result.AsSpan(136 + 52, 4).ToArray().Should().Equal(0, 0, 0, 136 + ruleLength);
        // and the list in it is any, with one rule
        result[136 + 56 + 11].Should().Be(1);
        result[136 + 56 + 15].Should().Be(1);
    }

    #endregion

    #region strings

    [TestCase("")]
    [TestCase("Waveshaper")]
    [TestCase("Ünïcödé")]
    [TestCase("日本語のアーティスト")]
    [TestCase("Fish & Chips <3 \"quoted\"")]
    [TestCase("emoji 😀 and 𝄞")]
    public void Encode_GivenAString_ReadsBackTheSame(string value) =>
        AssertRoundTrips(And(Rules(new StringRule(StringFields.Album, Operator.Contains, Sign.StringPositive, value))));

    [Test]
    public void Encode_GivenAStringWithCharactersOutsideLatin1_WritesBigEndianUtf16()
    {
        var result = CriteriaEncoder.Encode(And(Rules(
            new StringRule(StringFields.Artist, Operator.Is, Sign.StringPositive, "日本"))));

        // 日 is U+65E5, 本 is U+672C. The number of bytes comes before them.
        result.Skip(136 + 52).Should().Equal(0, 0, 0, 4, 0x65, 0xE5, 0x67, 0x2C);
    }

    [Test]
    public void Parse_GivenAStringThatIsNotLatin1_ReadsIt()
    {
        var criteria = SmartPlaylistTestData.ListOfRules(2, false,
            SmartPlaylistTestData.StringRule(field: 0x04, sign: 1, @operator: 2, "日本語"),
            SmartPlaylistTestData.StringRule(field: 0x03, sign: 3, @operator: 4, "after"));

        var result = CriteriaParser.Parse(criteria);

        result.Rules.Should().BeEquivalentTo(new IRule[]
        {
            new StringRule(StringFields.Artist, Operator.Contains, Sign.StringPositive, "日本語"),
            new StringRule(StringFields.Album, Operator.Starts, Sign.StringNegative, "after"),
        }, options => options.IncludingAllRuntimeProperties().WithStrictOrdering());
    }

    [TestCase(Operator.Is)]
    [TestCase(Operator.Contains)]
    [TestCase(Operator.Starts)]
    [TestCase(Operator.Ends)]
    public void Encode_GivenEachStringOperator_ReadsBackTheSame(Operator @operator) =>
        AssertRoundTrips(And(Rules(
            new StringRule(StringFields.Name, @operator, Sign.StringNegative, "x"))));

    #endregion

    #region numbers

    [Test]
    public void Encode_GivenARatingRange_StoresTwentyForEachStarUpToTheEndOfTheLastOne()
    {
        var result = CriteriaEncoder.Encode(And(Rules(
            new IntRule(IntFields.Rating, Operator.Other, Sign.IntPositive, 3, 5))));

        var rule = result.Skip(136).ToArray();
        rule[6].Should().Be(1, "it is a range");
        rule.Skip(56 + 4).Take(4).Should().Equal(0, 0, 0, 60);
        rule.Skip(56 + 28).Take(4).Should().Equal(0, 0, 0, 109);
    }

    [Test]
    public void Encode_GivenNoRating_StoresTheRangeFromZeroToNine() =>
        // it is how iTunes stores "not rated"
        CriteriaEncoder.Encode(And(Rules(new IntRule(IntFields.Rating, Operator.Other, Sign.IntPositive, 0, 0))))
            .Skip(136 + 56 + 28).Take(4).Should().Equal(0, 0, 0, 9);

    [TestCase(Operator.Greater, 29)] // above all of the 1 star ratings, as iTunes does
    [TestCase(Operator.Less, 20)] // below all of the 1 star ratings
    public void Encode_GivenARatingGreaterOrLessThan_StoresWhereTheStarsBegin(Operator @operator, byte expected)
    {
        var rule = CriteriaEncoder.Encode(And(Rules(
            new IntRule(IntFields.Rating, @operator, Sign.IntPositive, 1, null)))).Skip(136).ToArray();

        rule.Skip(56 + 4).Take(4).Should().Equal(0, 0, 0, expected);
        rule.Skip(56 + 28).Take(4).Should().Equal(0, 0, 0, expected);
    }

    [Test]
    public void Parse_GivenTheRatingRulesITunesMakes_ReadsThem()
    {
        var (_, criteria) = SmartPlaylistTestData.Blobs(SmartPlaylistTestData.Load("TestRatings"));

        var rules = CriteriaParser.Parse(criteria).SubConjunctions.Last().Rules;

        rules.Should().BeEquivalentTo(new IRule[]
        {
            new IntRule(IntFields.Rating, Operator.Other, Sign.IntPositive, 5, 5),
            new IntRule(IntFields.Rating, Operator.Other, Sign.IntNegative, 1, 1),
            new IntRule(IntFields.Rating, Operator.Greater, Sign.IntPositive, 1, null),
            new IntRule(IntFields.Rating, Operator.Less, Sign.IntPositive, 5, null),
        }, options => options.IncludingAllRuntimeProperties().WithStrictOrdering());
    }

    [Test]
    public void Parse_GivenTheDateRulesITunesMakes_ReadsThem()
    {
        var (_, criteria) = SmartPlaylistTestData.Blobs(SmartPlaylistTestData.Load("TestDates"));

        var rules = CriteriaParser.Parse(criteria).SubConjunctions.Last().Rules;

        // chosen in iTunes as the whole of 28 November 2026, a time zone without daylight saving
        rules.Should().BeEquivalentTo(new IRule[]
        {
            new DateRule(DateFields.DateAdded, Operator.Other, Sign.IntPositive,
                new DateTime(2026, 11, 28, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2026, 11, 28, 23, 59, 59, DateTimeKind.Utc)),
            new DateRule(DateFields.LastSkipped, Operator.Greater, Sign.IntPositive,
                new DateTime(2026, 11, 28, 23, 59, 59, DateTimeKind.Utc), null),
            new TimeSpanRule(DateFields.DateModified, Operator.Other, Sign.IntNegative, TimeSpan.FromDays(1)),
            new TimeSpanRule(DateFields.LastPlayed, Operator.Other, Sign.IntPositive, TimeSpan.FromDays(14)),
        }, options => options.IncludingAllRuntimeProperties().WithStrictOrdering());
    }

    [Test]
    public void Encode_GivenAnIntRuleThatIsNotARange_StoresTheValueAsBothEnds()
    {
        var rule = CriteriaEncoder.Encode(And(Rules(
            new IntRule(IntFields.Plays, Operator.Greater, Sign.IntPositive, 25, null)))).Skip(136).ToArray();

        rule[7].Should().Be(0x10);
        rule.Skip(56).Take(8).Should().Equal(0, 0, 0, 0, 0, 0, 0, 25);
        rule.Skip(56 + 24).Take(8).Should().Equal(0, 0, 0, 0, 0, 0, 0, 25);
    }

    [TestCase(Operator.Is, null)]
    [TestCase(Operator.Greater, null)]
    [TestCase(Operator.Less, null)]
    [TestCase(Operator.Other, 40L)]
    public void Encode_GivenEachIntOperator_ReadsBackTheSame(Operator @operator, long? valueB) =>
        AssertRoundTrips(And(Rules(
            new IntRule(IntFields.Year, @operator, Sign.IntNegative, 1999, valueB))));

    [TestCase(Operator.Is, null)]
    [TestCase(Operator.Other, 5L)]
    public void Encode_GivenRatings_ReadsBackTheSame(Operator @operator, long? valueB) =>
        AssertRoundTrips(And(Rules(new IntRule(IntFields.Rating, @operator, Sign.IntPositive, 2, valueB))));

    [Test]
    public void Encode_GivenALargeNumber_ReadsBackTheSame() =>
        AssertRoundTrips(And(Rules(new IntRule(IntFields.Size, Operator.Greater, Sign.IntPositive, int.MaxValue, null))));

    [Test]
    public void Encode_GivenARangeWithoutAnEnd_Throws()
    {
        var act = () => CriteriaEncoder.Encode(And(Rules(
            new IntRule(IntFields.Year, Operator.Other, Sign.IntPositive, 1999, null))));

        act.Should().Throw<ArgumentException>().WithMessage("*two values*");
    }

    #endregion

    #region dates

    [Test]
    public void Encode_GivenADate_StoresSecondsSince1904()
    {
        // 1 Jan 2020 is 3,660,681,600 seconds after 1 Jan 1904
        var rule = CriteriaEncoder.Encode(And(Rules(new DateRule(DateFields.DateModified, Operator.Greater,
            Sign.IntPositive, new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc), null)))).Skip(136).ToArray();

        rule.Skip(56 + 4).Take(4).Should().Equal(BitConverter.GetBytes(3660681600u).Reverse());
    }

    [Test]
    public void Parse_GivenADateInTheLastCentury_ReadsTheDate()
    {
        // 1 Jan 1970 is 2,082,844,800 seconds after 1 Jan 1904
        var criteria = CriteriaEncoder.Encode(And(Rules(new DateRule(DateFields.DateAdded, Operator.Less,
            Sign.IntPositive, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), null))));

        criteria.Skip(136 + 56 + 4).Take(4).Should().Equal(BitConverter.GetBytes(2082844800u).Reverse());
        ((DateRule) CriteriaParser.Parse(criteria).Rules.Single()).ValueA.Should()
            .Be(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
    }

    [TestCase(Operator.Is, false)]
    [TestCase(Operator.Greater, false)]
    [TestCase(Operator.Less, false)]
    [TestCase(Operator.Other, true)]
    public void Encode_GivenEachDateOperator_ReadsBackTheSame(Operator @operator, bool isRange) =>
        AssertRoundTrips(And(Rules(new DateRule(DateFields.LastPlayed, @operator, Sign.IntNegative,
            new DateTime(2019, 6, 1, 12, 30, 15, DateTimeKind.Utc),
            isRange ? new DateTime(2021, 2, 3, 4, 5, 6, DateTimeKind.Utc) : null))));

    [Test]
    public void Encode_GivenALocalDate_StoresItAsTheSameMoment()
    {
        var local = new DateTime(2020, 6, 1, 12, 0, 0, DateTimeKind.Local);

        var result = CriteriaParser.Parse(CriteriaEncoder.Encode(And(Rules(
            new DateRule(DateFields.DateAdded, Operator.Is, Sign.IntPositive, local, null)))));

        ((DateRule) result.Rules.Single()).ValueA.ToUniversalTime().Should().Be(local.ToUniversalTime());
    }

    [Test]
    public void Encode_GivenADateTooLateToStore_Throws()
    {
        var act = () => CriteriaEncoder.Encode(And(Rules(new DateRule(DateFields.DateAdded, Operator.Is,
            Sign.IntPositive, new DateTime(2100, 1, 1, 0, 0, 0, DateTimeKind.Utc), null))));

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region time spans

    [TestCase(2, TimeUnits.Months, 2 * 2628000)]
    [TestCase(2, TimeUnits.Weeks, 2 * 604800)]
    [TestCase(10, TimeUnits.Days, 10 * 86400)]
    public void Encode_GivenATimeSpan_StoresANumberOfTheLargestUnitThatFits(int number, TimeUnits unit, int seconds)
    {
        var rule = CriteriaEncoder.Encode(And(Rules(new TimeSpanRule(DateFields.LastPlayed, Operator.Other,
            Sign.IntPositive, TimeSpan.FromSeconds(seconds))))).Skip(136).ToArray();

        rule[6].Should().Be(2, "it is an 'in the last'");
        // the number of units before now, so below 0
        rule.Skip(56 + 8).Take(8).Should().Equal(BitConverter.GetBytes((long) -number).Reverse());
        rule.Skip(56 + 16).Take(8).Should().Equal(BitConverter.GetBytes((long) unit).Reverse());
    }

    [Test]
    public void Encode_GivenATimeSpan_FillsTheUnusedValuesTheWayITunesDoes()
    {
        var rule = CriteriaEncoder.Encode(And(Rules(new TimeSpanRule(DateFields.DateAdded, Operator.Other,
            Sign.IntPositive, TimeSpan.FromDays(7))))).Skip(136).ToArray();

        var unused = new byte[] {0x2d, 0xae, 0x2d, 0xae, 0x2d, 0xae, 0x2d, 0xae};
        rule.Skip(56).Take(8).Should().Equal(unused);
        rule.Skip(56 + 24).Take(8).Should().Equal(unused);
    }

    [TestCase(1)]
    [TestCase(3)]
    [TestCase(14)]
    [TestCase(30)]
    [TestCase(7 * 4)]
    [TestCase(91.25)] // 3 months
    public void Encode_GivenATimeSpan_ReadsBackTheSame(double days) =>
        AssertRoundTrips(And(Rules(new TimeSpanRule(DateFields.DateAdded, Operator.Other, Sign.IntNegative,
            TimeSpan.FromDays(days)))));

    [Test]
    public void Encode_GivenATimeSpanThatIsNotWholeDays_Throws()
    {
        var act = () => CriteriaEncoder.Encode(And(Rules(new TimeSpanRule(DateFields.DateAdded, Operator.Other,
            Sign.IntPositive, TimeSpan.FromHours(36)))));

        act.Should().Throw<ArgumentException>().WithMessage("*whole number of days*");
    }

    #endregion

    #region booleans, lists of values and playlists

    [TestCase(BoolFields.Checked, Sign.IntPositive)]
    [TestCase(BoolFields.Purchased, Sign.IntNegative)]
    [TestCase(BoolFields.HasArtwork, Sign.IntPositive)]
    public void Encode_GivenABoolean_ReadsBackTheSame(BoolFields field, Sign sign) =>
        AssertRoundTrips(And(Rules(new BooleanRule(field, Operator.Is, sign))));

    [TestCase(DictionaryFields.MediaKind, "Podcast", Operator.Is)]
    [TestCase(DictionaryFields.MediaKind, "Music Video", Operator.Is)]
    [TestCase(DictionaryFields.MediaKind, "Undesired Music", Operator.Other)]
    [TestCase(DictionaryFields.Location, "iCloud", Operator.Is)]
    [TestCase(DictionaryFields.Location, "Computer", Operator.Other)]
    [TestCase(DictionaryFields.Love, "Loved", Operator.Is)]
    [TestCase(DictionaryFields.iCloudStatus, "Matched", Operator.Is)]
    public void Encode_GivenAValueFromAList_ReadsBackTheSame(DictionaryFields field, string value, Operator @operator) =>
        AssertRoundTrips(And(Rules(new DictionaryRule(field, @operator, Sign.IntPositive, value))));

    [TestCase(Operator.Is, 0x01)]
    [TestCase(Operator.Other, 0x1021B1)]
    public void Encode_GivenMusic_UsesTheNumberThatTheKindOfRuleUses(Operator @operator, int expected)
    {
        // the playlist with all music, and a playlist that has been chosen to have music, say "Music" differently
        var rule = CriteriaEncoder.Encode(And(Rules(
            new DictionaryRule(DictionaryFields.MediaKind, @operator, Sign.IntPositive, "Music")))).Skip(136).ToArray();

        rule.Skip(56 + 4).Take(4).Should().Equal(BitConverter.GetBytes(expected).Reverse());
        rule[6].Should().Be(@operator == Operator.Other ? (byte) 4 : (byte) 0);
    }

    [Test]
    public void Encode_GivenAValueThatIsNotInTheList_Throws()
    {
        var act = () => CriteriaEncoder.Encode(And(Rules(
            new DictionaryRule(DictionaryFields.Love, Operator.Is, Sign.IntPositive, "Adored"))));

        act.Should().Throw<ArgumentException>().WithMessage("*'Adored' is not a value of Love*");
    }

    [Test]
    public void Encode_GivenARuleAboutAPlaylist_SaysItIsNotSupported()
    {
        var act = () => CriteriaEncoder.Encode(And(Rules(
            new PlaylistRule(PlaylistFields.PlaylistPersistentId, Operator.Is, Sign.IntPositive, "0AB4B9146BF18882"))));

        act.Should().Throw<NotSupportedException>();
    }

    #endregion

    #region groups and order

    [Test]
    public void Encode_GivenAnyAndAll_ReadsBackTheSame() =>
        AssertRoundTrips(Or(Rules(new StringRule(StringFields.Genre, Operator.Is, Sign.StringPositive, "Jazz")),
            And(Rules(new IntRule(IntFields.Year, Operator.Greater, Sign.IntPositive, 1990, null),
                    new IntRule(IntFields.Year, Operator.Less, Sign.IntPositive, 2000, null)),
                Or(Rules(new StringRule(StringFields.Artist, Operator.Starts, Sign.StringPositive, "A"),
                    new StringRule(StringFields.Artist, Operator.Ends, Sign.StringNegative, "Z"))))));

    [Test]
    public void Encode_GivenNothing_ReadsBackNothing() => AssertRoundTrips(And(Rules()));

    [Test]
    public void Encode_GivenRulesAndGroups_WritesRulesFirst()
    {
        var group = And(Rules(new StringRule(StringFields.Genre, Operator.Is, Sign.StringPositive, "Jazz")));
        var rule = new IntRule(IntFields.Year, Operator.Is, Sign.IntPositive, 2000, null);

        var result = CriteriaEncoder.Encode(And(Rules(rule), group));

        result[136 + 3].Should().Be((byte) IntFields.Year);
    }

    private static readonly Conjunction _group =
        Or(Rules(new StringRule(StringFields.Genre, Operator.Is, Sign.StringPositive, "Jazz")));

    private static readonly IntRule _rule = new(IntFields.Year, Operator.Is, Sign.IntPositive, 2000, null);

    /// <summary>
    /// Criteria with a group, and then a rule. This is an order that can't be told from the one with a rule and then a
    /// group once it is read.
    /// </summary>
    private static byte[] GroupThenRule()
    {
        var group = CriteriaEncoder.Encode(And(Rules(), _group)).Skip(136).ToArray();
        var rule = CriteriaEncoder.Encode(And(Rules(_rule))).Skip(136).ToArray();

        return SmartPlaylistTestData.ListOfRules(2, false, group, rule);
    }

    [Test]
    public void Encode_GivenTheExistingCriteria_KeepsTheOrderOfRulesAndGroups()
    {
        var existing = GroupThenRule();
        var conjunction = CriteriaParser.Parse(existing);

        CriteriaEncoder.Encode(conjunction).Should().NotEqual(existing, "it would put the rule first");
        CriteriaEncoder.Encode(conjunction, existing).Should().Equal(existing);
    }

    [Test]
    public void Encode_GivenAnEditedListAndTheExistingCriteria_KeepsTheOrderOfOtherLists()
    {
        var existing = GroupThenRule();
        var conjunction = CriteriaParser.Parse(existing);

        // a rule is added to the group, which doesn't change the order of the list that it is in
        var edited = conjunction with
        {
            SubConjunctions = [conjunction.SubConjunctions.Single() with
            {
                Rules = Rules(_group.Rules.Single(),
                    new StringRule(StringFields.Artist, Operator.Is, Sign.StringPositive, "Added"))
            }]
        };

        var result = CriteriaEncoder.Encode(edited, existing);

        result[136 + 3].Should().Be(0, "the group is still first");
        CriteriaParser.Parse(result).Should().BeEquivalentTo(edited, o => o.IncludingAllRuntimeProperties());
    }

    [Test]
    public void Encode_GivenExistingCriteriaThatDoesNotMatchTheListsInIt_WritesRulesFirst()
    {
        var existing = GroupThenRule();

        // the list now has two rules and no group, which isn't what the existing criteria has
        var result = CriteriaEncoder.Encode(And(Rules(_rule, _rule)), existing);

        CriteriaParser.Parse(result).Rules.Should().HaveCount(2);
    }

    [TestCase(new byte[0])]
    [TestCase(new byte[] {1, 2, 3})]
    public void Encode_GivenExistingCriteriaThatIsNotCriteria_IgnoresIt(byte[] existing)
    {
        var conjunction = And(Rules(_rule), _group);

        CriteriaEncoder.Encode(conjunction, existing).Should().Equal(CriteriaEncoder.Encode(conjunction));
    }

    #endregion

    #region rejecting rules

    [Test]
    public void Encode_GivenAStringRuleWithAnOperatorThatIsForNumbers_Throws()
    {
        var act = () => CriteriaEncoder.Encode(And(Rules(
            new StringRule(StringFields.Artist, Operator.Greater, Sign.StringPositive, "x"))));

        act.Should().Throw<ArgumentException>().WithMessage("*StringRule can't use the operator Greater*");
    }

    [Test]
    public void Encode_GivenAStringRuleWithASignForNumbers_Throws()
    {
        var act = () => CriteriaEncoder.Encode(And(Rules(
            new StringRule(StringFields.Artist, Operator.Is, Sign.IntPositive, "x"))));

        act.Should().Throw<ArgumentException>().WithMessage("*StringRule can't use the sign IntPositive*");
    }

    [Test]
    public void Encode_GivenANumberRuleWithASignForStrings_Throws()
    {
        var act = () => CriteriaEncoder.Encode(And(Rules(
            new IntRule(IntFields.Year, Operator.Is, Sign.StringPositive, 1, null))));

        act.Should().Throw<ArgumentException>().WithMessage("*IntRule can't use the sign StringPositive*");
    }

    [Test]
    public void Encode_GivenATimeSpanThatIsNotInTheLast_Throws()
    {
        var act = () => CriteriaEncoder.Encode(And(Rules(
            new TimeSpanRule(DateFields.DateAdded, Operator.Is, Sign.IntPositive, TimeSpan.FromDays(1)))));

        act.Should().Throw<ArgumentException>();
    }

    #endregion
}
