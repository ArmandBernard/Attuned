using iTunesSmartParser.Data;
using iTunesSmartParser.Data.Limits;
using iTunesSmartParser.Data.Logic;
using iTunesSmartParser.Data.Rules;
using iTunesSmartParser.Fields;

namespace iTunesParserTests.Xml.TestPlaylists;

public static class ExpectedPlaylistOutputs
{
    private static readonly Conjunction MusicMediaConjunction = new(ConjunctionType.Or, Array.Empty<Conjunction>(),
        new List<IRule>
        {
            new DictionaryRule(DictionaryFields.MediaKind, Operator.Is, Sign.IntPositive, "Music"),
            new DictionaryRule(DictionaryFields.MediaKind, Operator.Is, Sign.IntPositive,
                "Music Video")
        });

    private static readonly Limit DefaultOffLimit =
        new(false, LimitUnits.Items, 25, false, SelectionMethods.Random, true);

    public static Playlist BestOfWaveshaper = new("Best of Waveshaper", 22432, new[] {7914, 7916, 7918}, true,
        new SmartPlaylistInformation(
            DefaultOffLimit,
            new Conjunction(ConjunctionType.And, new List<Conjunction>()
            {
                MusicMediaConjunction,
                new(ConjunctionType.And, Array.Empty<Conjunction>(), new List<IRule>()
                {
                    new StringRule(StringFields.Artist, Operator.Contains, Sign.StringPositive, "Waveshaper"),
                    new IntRule(IntFields.Rating, Operator.Other, Sign.IntPositive, 3, 5)
                })
            }, Array.Empty<IRule>()),
            true
        ));

    public static readonly Playlist FavouriteClassical = new(
        "favourite classical",
        22530,
        new[] {1082, 1054, 1042},
        true,
        new SmartPlaylistInformation(
            DefaultOffLimit,
            new Conjunction(ConjunctionType.And, new List<Conjunction>()
            {
                MusicMediaConjunction,
                new(ConjunctionType.And, new List<Conjunction>
                {
                    new(ConjunctionType.Or, Array.Empty<Conjunction>(), new List<IRule>()
                    {
                        new StringRule(StringFields.Genre, Operator.Contains, Sign.StringPositive, "Classical"),
                        new StringRule(StringFields.Artist, Operator.Contains, Sign.StringPositive, "Einaudi"),
                        new StringRule(StringFields.Album, Operator.Contains, Sign.StringPositive,
                            "classical music"),
                    })
                }, new List<IRule>()
                {
                    new IntRule(IntFields.Rating, Operator.Other, Sign.IntPositive, 3, 5)
                })
            }, Array.Empty<IRule>()),
            true
        ));

    public static readonly Playlist MostPlayed = new(
        "Most played",
        22804,
        new[] {724, 2528, 820},
        true,
        new SmartPlaylistInformation(
            new Limit(true, LimitUnits.Items, 50, false, SelectionMethods.OftenPlayed, true),
            null, true)
    );
}