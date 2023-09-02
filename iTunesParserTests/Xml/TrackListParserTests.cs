using FluentAssertions;
using iTunesSmartParser.Xml;
using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesParserTests.Xml;

public class TrackListParserTests
{
    // lang=XML
    private const string TestDoc =
        """
        <?xml version="1.0" encoding="UTF-8"?>
        <!DOCTYPE plist PUBLIC "-//Apple Computer//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
        <plist version="1.0">
        <dict>
            <key>Major Version</key><integer>1</integer>
            <key>Minor Version</key><integer>1</integer>
            <key>Application Version</key><string>12.9.0.167</string>
            <key>Date</key><date>2018-09-18T23:48:44Z</date>
            <key>Features</key><integer>5</integer>
            <key>Show Content Ratings</key><true/>
            <key>Library Persistent ID</key><string>CE48DC6FF2C9F4A1</string>
            <key>Tracks</key>
            <dict>
                <key>628</key>
                <dict>
                    <key>Track ID</key><integer>628</integer>
                    <key>Size</key><integer>3653055</integer>
                    <key>Total Time</key><integer>220368</integer>
                    <key>Date Modified</key><date>2014-12-26T14:17:09Z</date>
                    <key>Date Added</key><date>2013-12-13T14:07:35Z</date>
                    <key>Bit Rate</key><integer>128</integer>
                    <key>Sample Rate</key><integer>44100</integer>
                    <key>Play Count</key><integer>56</integer>
                    <key>Play Date</key><integer>3618396162</integer>
                    <key>Play Date UTC</key><date>2018-08-29T13:02:42Z</date>
                    <key>Skip Count</key><integer>3</integer>
                    <key>Skip Date</key><date>2014-05-22T12:47:45Z</date>
                    <key>Rating</key><integer>100</integer>
                    <key>Album Rating</key><integer>100</integer>
                    <key>Album Rating Computed</key><true/>
                    <key>Artwork Count</key><integer>1</integer>
                    <key>Persistent ID</key><string>F2F72814A45D91ED</string>
                    <key>Track Type</key><string>File</string>
                    <key>File Folder Count</key><integer>-1</integer>
                    <key>Library Folder Count</key><integer>-1</integer>
                    <key>Name</key><string>Resonance</string>
                    <key>Artist</key><string>TM Revolution</string>
                    <key>Album</key><string>Soul Eater</string>
                    <key>Genre</key><string>Anime</string>
                    <key>Kind</key><string>MPEG audio file</string>
                    <key>Location</key><string>file://localhost/D:/Users/arman/Documents/MEGA/MUSIC/Soul%20Eater/Resonance.mp3</string>
                </dict>
                <key>630</key>
                <dict>
                    <key>Track ID</key><integer>630</integer>
                    <key>Size</key><integer>11230399</integer>
                    <key>Total Time</key><integer>277577</integer>
                    <key>Track Number</key><integer>1</integer>
                    <key>Year</key><integer>2009</integer>
                    <key>Date Modified</key><date>2014-12-26T14:17:09Z</date>
                    <key>Date Added</key><date>2013-12-13T14:07:35Z</date>
                    <key>Bit Rate</key><integer>320</integer>
                    <key>Sample Rate</key><integer>44100</integer>
                    <key>Play Count</key><integer>99</integer>
                    <key>Play Date</key><integer>3611163394</integer>
                    <key>Play Date UTC</key><date>2018-06-06T19:56:34Z</date>
                    <key>Skip Count</key><integer>12</integer>
                    <key>Skip Date</key><date>2018-08-30T16:11:23Z</date>
                    <key>Rating</key><integer>100</integer>
                    <key>Album Rating</key><integer>100</integer>
                    <key>Album Rating Computed</key><true/>
                    <key>Loved</key><true/>
                    <key>Artwork Count</key><integer>1</integer>
                    <key>Persistent ID</key><string>472BE9B7ABA2F8F1</string>
                    <key>Track Type</key><string>File</string>
                    <key>File Folder Count</key><integer>-1</integer>
                    <key>Library Folder Count</key><integer>-1</integer>
                    <key>Name</key><string>STRENGTH.</string>
                    <key>Artist</key><string>Abingdon Boys School</string>
                    <key>Album</key><string>Soul Eater</string>
                    <key>Genre</key><string>Anime</string>
                    <key>Kind</key><string>MPEG audio file</string>
                    <key>Location</key><string>file://localhost/D:/Users/arman/Documents/MEGA/MUSIC/Soul%20Eater/STRENGTH_.mp3</string>
                </dict>
            </dict>
        </dict>
        </plist>
        """;

    private readonly TrackListParser _trackListParser = new();

    private readonly IEnumerable<Track> _expectedTrackList = new Track[]
    {
        new()
        {
            Album = "Soul Eater",
            Artist = "TM Revolution",
            Bpm = null,
            BitRate = 128,
            Channels = null,
            Codec = null,
            Composer = null,
            CoverArt = null,
            DateAdded = DateTime.Parse("2013-12-13 14:07:35"),
            DateModified = DateTime.Parse("2014-12-26 14:17:09"),
            DiscCount = null,
            DiscNumber = null,
            Genre = "Anime",
            Location = "file://localhost/D:/Users/arman/Documents/MEGA/MUSIC/Soul%20Eater/Resonance.mp3",
            Loved = false,
            Name = "Resonance",
            PlayCount = 56,
            PlayDate = DateTime.Parse("2018-08-29 14:02:42"),
            Rating100 = 100,
            SampleRate = 44100,
            Size = 3653055,
            SkipCount = 3,
            TotalTime = TimeSpan.FromMilliseconds(220368),
            TrackCount = null,
            Id = 628,
            TrackNumber = null,
            Type = null,
            Year = null
        },
        new()
        {
            Album = "Soul Eater",
            Artist = "Abingdon Boys School",
            Bpm = null,
            BitRate = 320,
            Channels = null,
            Codec = null,
            Composer = null,
            CoverArt = null,
            DateAdded = DateTime.Parse("2013-12-13 14:07:35"),
            DateModified = DateTime.Parse("2014-12-26 14:17:09"),
            DiscCount = null,
            DiscNumber = null,
            Genre = "Anime",
            Location = "file://localhost/D:/Users/arman/Documents/MEGA/MUSIC/Soul%20Eater/STRENGTH_.mp3",
            Loved = true,
            Name = "STRENGTH.",
            PlayCount = 99,
            PlayDate = DateTime.Parse("2018-06-06 20:56:34"),
            Rating100 = 100,
            SampleRate = 44100,
            Size = 11230399,
            SkipCount = 12,
            TotalTime = TimeSpan.FromMilliseconds(277577),
            TrackCount = null,
            Id = 630,
            TrackNumber = 1,
            Type = null,
            Year = 2009
        }
    };

    [Test]
    public void TrackListParser_GivenValidDocument_ShouldReturnExpectedTrackList()
    {
        var doc = XDocument.Parse(TestDoc);

        var result = _trackListParser.ParseTracks(doc);

        result.Should().BeEquivalentTo(_expectedTrackList);
    }
}