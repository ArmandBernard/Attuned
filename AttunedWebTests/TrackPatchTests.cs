using System.Net;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using AttunedWebApi.Controllers;
using AttunedWebApi.Filters;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AttunedWebTests;

/// <summary>
/// End to end tests of PATCH /Track/{id}, running the real app against a temporary library file.
/// </summary>
[TestFixture]
public class TrackPatchTests
{
    // lang=XML
    private const string LIBRARY =
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
                    <key>Name</key><string>Original name</string>
                    <key>Artist</key><string>Original artist</string>
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

    private string _directory = null!;
    private string _libraryPath = null!;
    // any type from the web assembly will do: Program is static so cannot be used
    private WebApplicationFactory<TrackController> _factory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void SetUp()
    {
        _directory = Directory.CreateTempSubdirectory().FullName;
        _libraryPath = Path.Combine(_directory, "Library.xml");
        File.WriteAllText(_libraryPath, LIBRARY);

        // Program reads this while building, so it has to be set before the app starts
        Environment.SetEnvironmentVariable("LibraryXml", _libraryPath);

        _factory = new WebApplicationFactory<TrackController>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
        Environment.SetEnvironmentVariable("LibraryXml", null);
        Directory.Delete(_directory, true);
    }

    private static HttpRequestMessage PatchRequest(string path, string json, string? readonlyHeader = "false")
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, path)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        if (readonlyHeader != null)
        {
            request.Headers.Add(RequireWritableAttribute.HEADER_NAME, readonlyHeader);
        }

        return request;
    }

    private async Task<JsonElement> GetTrack(int id)
    {
        var tracks = await _client.GetFromJsonAsync<JsonElement>("/Track");
        return tracks.EnumerateArray().Single(x => x.GetProperty("Id").GetInt32() == id);
    }

    [Test]
    public async Task Patch_UpdatesOnlyTheSentProperties_AndReturnsTheTrack()
    {
        var response = await _client.SendAsync(PatchRequest("/Track/1", """{"Name":"New name"}"""));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("Name").GetString().Should().Be("New name");
        body.GetProperty("Artist").GetString().Should().Be("Original artist");
        body.GetProperty("Year").GetInt32().Should().Be(2009);
        body.GetProperty("Rating").GetInt32().Should().Be(5);
    }

    [Test]
    public async Task Patch_IsPersistedToTheLibraryFile()
    {
        await _client.SendAsync(PatchRequest("/Track/1", """{"Name":"New name","Rating":3}"""));

        var doc = XDocument.Load(_libraryPath);
        var text = doc.ToString();
        text.Should().Contain("New name");
        text.Should().Contain("<integer>60</integer>");
        text.Should().Contain("Original artist");
    }

    [Test]
    public async Task Patch_ChangesOnlyTheEditedLinesOfTheFile()
    {
        var before = (await File.ReadAllTextAsync(_libraryPath)).TrimEnd().ReplaceLineEndings("\n").Split('\n');

        await _client.SendAsync(PatchRequest("/Track/1", """{"Name":"New name","Bpm":120}"""));

        var after = (await File.ReadAllTextAsync(_libraryPath)).TrimEnd().ReplaceLineEndings("\n").Split('\n');
        after.Should().HaveCount(before.Length + 1, "one line is added, for the BPM");
        after.Except(before).Should().BeEquivalentTo(
            "            <key>Name</key><string>New name</string>",
            "            <key>BPM</key><integer>120</integer>");
        before.Except(after).Should().BeEquivalentTo(
            "            <key>Name</key><string>Original name</string>");
    }

    [Test]
    public async Task Patch_IsVisibleToLaterReads_EvenThoughTheTrackListIsCached()
    {
        // populate the cache first
        (await GetTrack(1)).GetProperty("Name").GetString().Should().Be("Original name");

        await _client.SendAsync(PatchRequest("/Track/1", """{"Name":"New name"}"""));

        (await GetTrack(1)).GetProperty("Name").GetString().Should().Be("New name");
    }

    [Test]
    public async Task Patch_DoesNotChangeOtherTracks()
    {
        await _client.SendAsync(PatchRequest("/Track/1", """{"Name":"New name"}"""));

        (await GetTrack(2)).GetProperty("Name").GetString().Should().Be("Other track");
    }

    [Test]
    public async Task Patch_NullClearsAProperty_WhereAbsentLeavesItAlone()
    {
        var response = await _client.SendAsync(PatchRequest("/Track/1", """{"Year":null}"""));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.TryGetProperty("Year", out _).Should().BeFalse("nulls are not written in responses");
        body.GetProperty("Name").GetString().Should().Be("Original name");
    }

    [Test]
    public async Task Patch_CanSetLovedAndRatingToZero()
    {
        var response = await _client.SendAsync(PatchRequest("/Track/1", """{"Loved":true,"Rating":0}"""));

        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("Loved").GetBoolean().Should().BeTrue();
        body.GetProperty("Rating").GetInt32().Should().Be(0);
    }

    [TestCase(null)]
    [TestCase("true")]
    [TestCase("")]
    [TestCase("nonsense")]
    public async Task Patch_WhenReadonlyModeIsNotExplicitlyOff_IsForbiddenAndWritesNothing(string? header)
    {
        var before = await File.ReadAllTextAsync(_libraryPath);

        var response = await _client.SendAsync(PatchRequest("/Track/1", """{"Name":"New name"}""", header));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        (await File.ReadAllTextAsync(_libraryPath)).Should().Be(before);
        File.Exists(_libraryPath + ".bak").Should().BeFalse();
    }

    [Test]
    public async Task Patch_ReadonlyHeaderIsCaseInsensitive()
    {
        var response = await _client.SendAsync(PatchRequest("/Track/1", """{"Name":"New name"}""", "False"));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task Patch_UnknownTrack_Is404_AndWritesNothing()
    {
        var before = await File.ReadAllTextAsync(_libraryPath);

        var response = await _client.SendAsync(PatchRequest("/Track/99", """{"Name":"New name"}"""));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await File.ReadAllTextAsync(_libraryPath)).Should().Be(before);
    }

    [TestCase("""{"Rating":6}""", TestName = "Patch_RatingAboveFive_IsRejected")]
    [TestCase("""{"Rating":-1}""", TestName = "Patch_NegativeRating_IsRejected")]
    [TestCase("""{"Rating":null}""", TestName = "Patch_NullRating_IsRejected")]
    [TestCase("""{"Year":-5}""", TestName = "Patch_NegativeYear_IsRejected")]
    [TestCase("""{"Year":"abc"}""", TestName = "Patch_WrongType_IsRejected")]
    [TestCase("""{"Location":"file://localhost/elsewhere.mp3"}""", TestName = "Patch_UnpatchableProperty_IsRejected")]
    [TestCase("""{"Name":"ok","Id":5}""", TestName = "Patch_UnpatchablePropertyAlongsideValidOne_IsRejected")]
    [TestCase("not json", TestName = "Patch_InvalidJson_IsRejected")]
    public async Task Patch_InvalidBody_Is400_AndWritesNothing(string json)
    {
        var before = await File.ReadAllTextAsync(_libraryPath);

        var response = await _client.SendAsync(PatchRequest("/Track/1", json));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await File.ReadAllTextAsync(_libraryPath)).Should().Be(before);
    }

    [Test]
    public async Task Patch_NothingToPatch_ReturnsOk()
    {
        var before = await File.ReadAllTextAsync(_libraryPath);

        var response = await _client.SendAsync(PatchRequest("/Track/1", "{}"));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await File.ReadAllTextAsync(_libraryPath)).Should().Be(before);
        File.Exists(_libraryPath + ".bak").Should().BeFalse("nothing was written");
    }

    [Test]
    public async Task Patch_NothingToPatch_ReturnsTheTrackAsItIs()
    {
        var response = await _client.SendAsync(PatchRequest("/Track/1", "{}"));

        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("Id").GetInt32().Should().Be(1);
        body.GetProperty("Name").GetString().Should().Be("Original name");
        body.GetProperty("Artist").GetString().Should().Be("Original artist");
    }

    [Test]
    public async Task Patch_NothingToPatch_UnknownTrack_Is404()
    {
        var response = await _client.SendAsync(PatchRequest("/Track/99", "{}"));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Patch_NothingToPatch_WhenReadonlyModeIsOn_IsStillForbidden()
    {
        // the read-only guard applies to the endpoint, whether or not there is anything to change
        var response = await _client.SendAsync(PatchRequest("/Track/1", "{}", readonlyHeader: null));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Patch_KeepsABackupOfTheOriginalLibrary()
    {
        await _client.SendAsync(PatchRequest("/Track/1", """{"Name":"First"}"""));
        await _client.SendAsync(PatchRequest("/Track/1", """{"Name":"Second"}"""));

        XNode.DeepEquals(XDocument.Load(_libraryPath + ".bak"), XDocument.Parse(LIBRARY)).Should().BeTrue();
    }

    [Test]
    public async Task Patch_ConcurrentPatchesToDifferentProperties_AreAllApplied()
    {
        var responses = await Task.WhenAll(
            Enumerable.Range(1, 10).Select(i =>
                _client.SendAsync(PatchRequest("/Track/1", $$"""{"Bpm":{{i}},"Composer":"c{{i}}"}"""))));
        responses.Should().OnlyContain(x => x.StatusCode == HttpStatusCode.OK);

        // writes are serialised, so the library is still valid and holds one complete patch
        var track = await GetTrack(1);
        track.GetProperty("Name").GetString().Should().Be("Original name");
        var bpm = track.GetProperty("Bpm").GetInt32();
        bpm.Should().BeInRange(1, 10);
        XDocument.Load(_libraryPath).ToString().Should().Contain($"c{bpm}");
    }
}
