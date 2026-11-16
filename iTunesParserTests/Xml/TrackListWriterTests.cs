using System.Xml.Linq;
using iTunesSmartParser.Data;
using iTunesSmartParser.Xml;

namespace iTunesParserTests.Xml;

[TestFixture]
public class TrackListWriterTests
{
    // lang=XML
    private const string TEST_DOC =
        """
        <?xml version="1.0" encoding="UTF-8"?>
        <!DOCTYPE plist PUBLIC "-//Apple Computer//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
        <plist version="1.0">
        <dict>
            <key>Tracks</key>
            <dict>
                <key>1</key>
                <dict>
                    <key>Track ID</key><integer>1</integer>
                    <key>Date Modified</key><date>2014-12-26T14:17:09Z</date>
                    <key>Date Added</key><date>2013-12-13T14:07:35Z</date>
                    <key>Rating</key><integer>100</integer>
                    <key>Loved</key><true/>
                    <key>Name</key><string>Original name</string>
                    <key>Artist</key><string>Original artist</string>
                    <key>Genre</key><string>Anime</string>
                    <key>Year</key><integer>2009</integer>
                    <key>Location</key><string>file://localhost/D:/one.mp3</string>
                </dict>
                <key>2</key>
                <dict>
                    <key>Track ID</key><integer>2</integer>
                    <key>Date Modified</key><date>2014-12-26T14:17:09Z</date>
                    <key>Date Added</key><date>2013-12-13T14:07:35Z</date>
                    <key>Name</key><string>Other track</string>
                    <key>Location</key><string>file://localhost/D:/two.mp3</string>
                </dict>
            </dict>
        </dict>
        </plist>
        """;

    private readonly ITrackListParser _parser = new TrackListParser();
    private TrackListWriter _writer = null!;
    private XDocument _doc = null!;

    [SetUp]
    public void SetUp()
    {
        _writer = new TrackListWriter(_parser);
        _doc = XDocument.Parse(TEST_DOC);
    }

    private Track GetTrack(int id) => _parser.ParseDocument(_doc).Single(x => x.Id == id);

    private XElement TrackElement(int id) =>
        _doc.Descendants("dict").First(x => x.PlistDictGet("Track ID")?.Value == id.ToString());

    [Test]
    public void ApplyPatch_UnknownTrack_ReturnsNullAndLeavesDocumentUnchanged()
    {
        var result = _writer.ApplyPatch(_doc, 99, new TrackPatch { Name = "New" });

        result.Should().BeNull();
        XNode.DeepEquals(_doc, XDocument.Parse(TEST_DOC)).Should().BeTrue();
    }

    [Test]
    public void ApplyPatch_EmptyPatch_LeavesDocumentUnchanged()
    {
        _writer.ApplyPatch(_doc, 1, new TrackPatch());

        XNode.DeepEquals(_doc, XDocument.Parse(TEST_DOC)).Should().BeTrue();
    }

    [Test]
    public void ApplyPatch_OnlyChangesProvidedProperties()
    {
        var before = GetTrack(1);

        _writer.ApplyPatch(_doc, 1, new TrackPatch { Name = "New name" });

        GetTrack(1).Should().BeEquivalentTo(before with { Name = "New name" });
    }

    [Test]
    public void ApplyPatch_DoesNotChangeOtherTracks()
    {
        var before = GetTrack(2);

        _writer.ApplyPatch(_doc, 1, new TrackPatch { Name = "New name", Rating = 1, Year = 1999 });

        GetTrack(2).Should().BeEquivalentTo(before);
    }

    [Test]
    public void ApplyPatch_ReturnsTheUpdatedTrack()
    {
        var result = _writer.ApplyPatch(_doc, 1, new TrackPatch { Artist = "New artist" });

        result.Should().BeEquivalentTo(GetTrack(1));
        result!.Artist.Should().Be("New artist");
    }

    [Test]
    public void ApplyPatch_SetsExistingAndMissingProperties()
    {
        _writer.ApplyPatch(_doc, 1, new TrackPatch
        {
            Name = "New name", // existing string
            Year = 2020, // existing int
            Composer = "A composer", // missing string
            Bpm = 140, // missing int
            AlbumArtist = "Someone",
            Album = "An album",
            DiscNumber = 1,
            DiscCount = 2,
            TrackNumber = 3,
            TrackCount = 12,
        });

        GetTrack(1).Should().BeEquivalentTo(new Track
        {
            Id = 1,
            Name = "New name",
            Year = 2020,
            Composer = "A composer",
            Bpm = 140,
            AlbumArtist = "Someone",
            Album = "An album",
            DiscNumber = 1,
            DiscCount = 2,
            TrackNumber = 3,
            TrackCount = 12,
            Artist = "Original artist",
            Genre = "Anime",
            Rating = 5,
            Loved = true,
            Location = "file://localhost/D:/one.mp3",
            DateModified = DateTime.Parse("2014-12-26T14:17:09Z"),
            DateAdded = DateTime.Parse("2013-12-13T14:07:35Z"),
        });
    }

    [Test]
    public void ApplyPatch_NullClearsTheProperty()
    {
        _writer.ApplyPatch(_doc, 1,
            new TrackPatch { Name = Option<string?>.Of(null), Year = Option<int?>.Of(null) });

        var track = GetTrack(1);
        track.Name.Should().BeNull();
        track.Year.Should().BeNull();
        TrackElement(1).Elements("key").Select(x => x.Value).Should().NotContain(["Name", "Year"]);
    }

    [Test]
    public void ApplyPatch_ClearingAMissingPropertyIsNotAnError()
    {
        var act = () => _writer.ApplyPatch(_doc, 2, new TrackPatch { Year = Option<int?>.Of(null) });

        act.Should().NotThrow();
    }

    [Test]
    public void ApplyPatch_ValuesAreStoredWithTheCorrectKeyAndType()
    {
        _writer.ApplyPatch(_doc, 2, new TrackPatch { Name = "Hello", Bpm = 120 });

        var dict = TrackElement(2);

        dict.PlistDictGet("Name")!.Name.LocalName.Should().Be("string");
        dict.PlistDictGet("BPM")!.Name.LocalName.Should().Be("integer");
        dict.PlistDictGet("BPM")!.Value.Should().Be("120");
    }

    [Test]
    public void ApplyPatch_StringsWithSpecialCharactersRoundTrip()
    {
        const string name = "<Fish & \"Chips\"> 日本語";

        _writer.ApplyPatch(_doc, 1, new TrackPatch { Name = name });

        var roundTripped = XDocument.Parse(_doc.ToString());
        _parser.ParseDocument(roundTripped).Single(x => x.Id == 1).Name.Should().Be(name);
    }

    [TestCase(1, 20)]
    [TestCase(3, 60)]
    [TestCase(5, 100)]
    public void ApplyPatch_RatingIsStoredOutOfOneHundred(int rating, int stored)
    {
        var result = _writer.ApplyPatch(_doc, 1, new TrackPatch { Rating = rating });

        result!.Rating.Should().Be(rating);
        TrackElement(1).PlistDictGet("Rating")!.Value.Should().Be(stored.ToString());
    }

    [Test]
    public void ApplyPatch_RatingOfZeroRemovesTheRating()
    {
        var result = _writer.ApplyPatch(_doc, 1, new TrackPatch { Rating = 0 });

        result!.Rating.Should().Be(0);
        TrackElement(1).Elements("key").Select(x => x.Value).Should().NotContain("Rating");
    }

    [Test]
    public void ApplyPatch_RatingCanBeSetOnAnUnratedTrack()
    {
        _writer.ApplyPatch(_doc, 2, new TrackPatch { Rating = 4 });

        GetTrack(2).Rating.Should().Be(4);
    }

    [TestCase(true, true)]
    [TestCase(false, false)]
    [TestCase(null, null)]
    public void ApplyPatch_Loved_SetsExpectedState(bool? loved, bool? expected)
    {
        // track 1 starts off loved, track 2 has neither flag
        foreach (var id in new[] { 1, 2 })
        {
            _writer.ApplyPatch(_doc, id, new TrackPatch { Loved = Option<bool?>.Of(loved) });

            GetTrack(id).Should().Match<Track>(x => x.Loved == expected);
        }
    }

    [Test]
    public void ApplyPatch_Loved_NeverLeavesBothFlags()
    {
        _writer.ApplyPatch(_doc, 1, new TrackPatch { Loved = false });
        _writer.ApplyPatch(_doc, 1, new TrackPatch { Loved = true });

        TrackElement(1).Elements("key").Select(x => x.Value).Should().Contain("Loved").And.NotContain("Disliked");
    }

    [Test]
    public void ApplyPatch_KeepsKeysAndValuesPaired()
    {
        _writer.ApplyPatch(_doc, 1, new TrackPatch
        {
            Name = Option<string?>.Of(null), Composer = "Someone", Year = 2001, Rating = 0,
            Loved = Option<bool?>.Of(null)
        });

        // the plist parser reads keys and values in alternating pairs, so this would throw if they got out of step
        var act = () => _parser.ParseDocument(_doc).ToList();

        act.Should().NotThrow();
    }
}
