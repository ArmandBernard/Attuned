using iTunesSmartParser;
using iTunesSmartParser.Data;
using iTunesSmartParser.Fields;

namespace iTunesParserTests.Xml.TestPlaylists;

public static class ExpectedPlaylistOutputs
{
    private static readonly Conjunction MusicMediaConjunction = new(ConjunctionType.Or, Array.Empty<Conjunction>(),
        new List<IRule>
        {
            new DictionaryRule(DictionaryFields.MediaKind, LogicRule.Is, LogicSign.IntPositive, "Music"),
            new DictionaryRule(DictionaryFields.MediaKind, LogicRule.Is, LogicSign.IntPositive,
                "Music Video")
        });

    private static readonly Limit DefaultOffLimit =
        new(false, LimitUnits.Items, 25, false, SelectionMethods.Random, true);

    public static Playlist BestOfWaveshaper = new("Best of Waveshaper", 22432, new[] {7914, 7916, 7918}, true,
        new PlaylistInformation(
            DefaultOffLimit,
            new Conjunction(ConjunctionType.And, new List<Conjunction>()
            {
                MusicMediaConjunction,
                new(ConjunctionType.And, Array.Empty<Conjunction>(), new List<IRule>()
                {
                    new StringRule(StringFields.Artist, LogicRule.Contains, LogicSign.StringPositive, "Waveshaper"),
                    new IntRule(IntFields.Rating, LogicRule.Other, LogicSign.IntPositive, 3, 5)
                })
            }, Array.Empty<IRule>()),
            true
        ));

    public static readonly Playlist FavouriteClassical = new(
        "favourite classical",
        22530,
        new[] {1082, 1054, 1042},
        true,
        new PlaylistInformation(
            DefaultOffLimit,
            new Conjunction(ConjunctionType.And, new List<Conjunction>()
            {
                MusicMediaConjunction,
                new(ConjunctionType.And, new List<Conjunction>
                {
                    new(ConjunctionType.Or, Array.Empty<Conjunction>(), new List<IRule>()
                    {
                        new StringRule(StringFields.Genre, LogicRule.Contains, LogicSign.StringPositive, "Classical"),
                        new StringRule(StringFields.Artist, LogicRule.Contains, LogicSign.StringPositive, "Einaudi"),
                        new StringRule(StringFields.Album, LogicRule.Contains, LogicSign.StringPositive,
                            "classical music"),
                    })
                }, new List<IRule>()
                {
                    new IntRule(IntFields.Rating, LogicRule.Other, LogicSign.IntPositive, 3, 5)
                })
            }, Array.Empty<IRule>()),
            true
        ));

    public static readonly Playlist MostPlayed = new(
        "Most played",
        22804,
        new[] {724, 2528, 820},
        true,
        new PlaylistInformation(
            new Limit(true, LimitUnits.Items, 50, false, SelectionMethods.OftenPlayed, true),
            null, true)
    );
}