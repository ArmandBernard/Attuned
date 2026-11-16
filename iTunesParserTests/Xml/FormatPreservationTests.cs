using iTunesSmartParser.Data;
using iTunesSmartParser.Xml;

namespace iTunesParserTests.Xml;

/// <summary>
/// Other programs read the library XML, so editing it should disturb as little of the file as possible: only the lines
/// that actually changed.
/// </summary>
[TestFixture]
public class FormatPreservationTests
{
    // Laid out the way iTunes writes it: UTF-8 declaration, tabs, CRLF line endings, "<true/>", and no final spaces.
    private static readonly string _library = string.Join("\r\n",
        """<?xml version="1.0" encoding="UTF-8"?>""",
        """<!DOCTYPE plist PUBLIC "-//Apple Computer//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">""",
        """<plist version="1.0">""",
        "<dict>",
        "\t<key>Show Content Ratings</key><true/>",
        "\t<key>Tracks</key>",
        "\t<dict>",
        "\t\t<key>1</key>",
        "\t\t<dict>",
        "\t\t\t<key>Track ID</key><integer>1</integer>",
        "\t\t\t<key>Date Modified</key><date>2014-12-26T14:17:09Z</date>",
        "\t\t\t<key>Date Added</key><date>2013-12-13T14:07:35Z</date>",
        "\t\t\t<key>Rating</key><integer>100</integer>",
        "\t\t\t<key>Loved</key><true/>",
        "\t\t\t<key>Name</key><string>Fish &#38; Chips &#60;3 日本語</string>",
        "\t\t\t<key>Year</key><integer>2009</integer>",
        "\t\t\t<key>Location</key><string>file://localhost/D:/one.mp3</string>",
        "\t\t</dict>",
        "\t\t<key>2</key>",
        "\t\t<dict>",
        "\t\t\t<key>Track ID</key><integer>2</integer>",
        "\t\t\t<key>Date Modified</key><date>2014-12-26T14:17:09Z</date>",
        "\t\t\t<key>Date Added</key><date>2013-12-13T14:07:35Z</date>",
        "\t\t\t<key>Name</key><string>Other track</string>",
        "\t\t\t<key>Location</key><string>file://localhost/D:/two.mp3</string>",
        "\t\t</dict>",
        "\t</dict>",
        "</dict>",
        "</plist>",
        "");

    private string _directory = null!;
    private string _path = null!;
    private ITunesXmlReader _xmlReader = null!;
    private ITunesXmlWriter _xmlWriter = null!;
    private readonly TrackListWriter _writer = new(new TrackListParser());

    [SetUp]
    public void SetUp()
    {
        _directory = Directory.CreateTempSubdirectory().FullName;
        _path = Path.Combine(_directory, "Library.xml");
        File.WriteAllBytes(_path, new System.Text.UTF8Encoding(false).GetBytes(_library));
        _xmlReader = new ITunesXmlReader(_path);
        _xmlWriter = new ITunesXmlWriter(_path);
    }

    [TearDown]
    public void TearDown() => Directory.Delete(_directory, true);

    private async Task Patch(int id, TrackPatch patch)
    {
        var doc = await _xmlReader.GetXDocument(CancellationToken.None);
        _writer.ApplyPatch(doc, id, patch);
        await _xmlWriter.SaveXDocument(doc, CancellationToken.None);
    }

    /// <summary>
    /// Compares two files, reporting where they first differ without dumping a whole library into the output
    /// </summary>
    private static void AssertSameBytes(byte[] expected, byte[] actual)
    {
        var index = 0;
        while (index < expected.Length && index < actual.Length && expected[index] == actual[index])
        {
            index++;
        }

        if (index == expected.Length && index == actual.Length)
        {
            return;
        }

        // hex, so that differences in invisible characters can be seen
        string Around(byte[] bytes) =>
            Convert.ToHexString(bytes[Math.Max(0, index - 20)..Math.Min(bytes.Length, index + 20)]);

        Assert.Fail($"Files first differ at byte {index} (sizes {expected.Length} and {actual.Length}).\n" +
                    $"Expected: {Around(expected)}\nActual:   {Around(actual)}");
    }

    private string[] Lines() => File.ReadAllText(_path).Split("\r\n");

    [Test]
    public async Task SavingAnUnchangedDocument_GivesBackIdenticalBytes()
    {
        var doc = await _xmlReader.GetXDocument(CancellationToken.None);

        await _xmlWriter.SaveXDocument(doc, CancellationToken.None);

        File.ReadAllBytes(_path).Should().Equal(File.ReadAllBytes(_path + ".bak"));
        File.ReadAllText(_path).Should().Be(_library);
    }

    [Test]
    public async Task EmptyPatch_GivesBackIdenticalBytes()
    {
        await Patch(1, new TrackPatch());

        File.ReadAllText(_path).Should().Be(_library);
    }

    [Test]
    public async Task ChangingAValue_ChangesOnlyThatLine()
    {
        await Patch(1, new TrackPatch { Year = 2020 });

        File.ReadAllText(_path).Should()
            .Be(_library.Replace("<key>Year</key><integer>2009</integer>", "<key>Year</key><integer>2020</integer>"));
    }

    [Test]
    public async Task AddingAValue_AddsOneIndentedLine()
    {
        await Patch(1, new TrackPatch { Bpm = 140 });

        var expected = _library.Replace(
            "\t\t\t<key>Location</key><string>file://localhost/D:/one.mp3</string>\r\n",
            "\t\t\t<key>Location</key><string>file://localhost/D:/one.mp3</string>\r\n" +
            "\t\t\t<key>BPM</key><integer>140</integer>\r\n");
        File.ReadAllText(_path).Should().Be(expected);
    }

    [Test]
    public async Task RemovingAValue_RemovesItsWholeLine()
    {
        await Patch(1, new TrackPatch { Year = Option<int?>.Of(null) });

        File.ReadAllText(_path).Should().Be(_library.Replace("\t\t\t<key>Year</key><integer>2009</integer>\r\n", ""));
    }

    [Test]
    public async Task ChangingLoved_UsesTheSelfClosingTagStyle()
    {
        await Patch(1, new TrackPatch { Loved = false });

        var lines = Lines();
        lines.Should().Contain("\t\t\t<key>Disliked</key><true/>");
        lines.Should().NotContain("\t\t\t<key>Loved</key><true/>");
        File.ReadAllText(_path).Should().NotContain("<true />");
    }

    [Test]
    public async Task EveryEditedLineIsIndentedLikeItsNeighbours()
    {
        await Patch(2, new TrackPatch
        {
            Rating = 4, Year = 1999, Composer = "Someone", Loved = true, Bpm = 90
        });

        var editedTrack = Lines().SkipWhile(x => !x.Contains("<key>Track ID</key><integer>2</integer>"))
            .TakeWhile(x => !x.Contains("</dict>")).ToList();
        editedTrack.Should().HaveCount(10).And.OnlyContain(x => x.StartsWith("\t\t\t<key>"));
    }

    [Test]
    public async Task OtherTracksAreByteForByteUntouched()
    {
        await Patch(1, new TrackPatch { Name = "A new name", Rating = 0 });

        var track2 = _library[_library.IndexOf("\t\t<key>2</key>", StringComparison.Ordinal)..];
        File.ReadAllText(_path).Should().EndWith(track2);
    }

    [Test]
    public async Task EditedFileIsStillReadableByTheParser()
    {
        await Patch(1, new TrackPatch { Name = "A new name", Composer = "Someone", Rating = 0 });

        var doc = await _xmlReader.GetXDocument(CancellationToken.None);
        var tracks = new TrackListParser().ParseDocument(doc).ToList();
        tracks.Should().HaveCount(2);
        tracks[0].Name.Should().Be("A new name");
        tracks[0].Composer.Should().Be("Someone");
        tracks[0].Rating.Should().Be(0);
    }

    [Test]
    public async Task FileIsWrittenWithoutAByteOrderMark()
    {
        await Patch(1, new TrackPatch { Year = 2020 });

        File.ReadAllBytes(_path).Take(3).Should().Equal("<?x"u8.ToArray());
    }

    [Test]
    [Explicit]
    [Category("LocalOnly")]
    // This test can only be run locally as a library is private
    public async Task SavingARealLibrary_GivesBackIdenticalBytes()
    {
        File.Copy("iTunesMusicLibrary.xml", _path, overwrite: true);

        var doc = await _xmlReader.GetXDocument(CancellationToken.None);
        await _xmlWriter.SaveXDocument(doc, CancellationToken.None);

        AssertSameBytes(File.ReadAllBytes(_path + ".bak"), File.ReadAllBytes(_path));
    }

    [Test]
    [Explicit]
    [Category("LocalOnly")]
    // This test can only be run locally as a library is private
    public async Task PatchingARealLibrary_ChangesOnlyTheEditedLines()
    {
        File.Copy("iTunesMusicLibrary.xml", _path, overwrite: true);
        var doc = await _xmlReader.GetXDocument(CancellationToken.None);
        var id = new TrackListParser().ParseDocument(doc).First().Id;

        await Patch(id, new TrackPatch { Bpm = 123, Rating = 3 });

        var before = File.ReadAllLines(_path + ".bak");
        var after = File.ReadAllLines(_path);
        // at most one line each for BPM and Rating
        after.Except(before).Count().Should().BeLessThanOrEqualTo(2);
        before.Except(after).Count().Should().BeLessThanOrEqualTo(1);
    }
}
