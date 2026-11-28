using System.Xml.Linq;
using iTunesSmartParser.Xml;

namespace iTunesParserTests.Xml;

[TestFixture]
public class ITunesXmlWriterTests
{
    private string _directory = null!;
    private string _path = null!;
    private ITunesXmlReader _reader = null!;
    private ITunesXmlWriter _writer = null!;

    [SetUp]
    public void SetUp()
    {
        _directory = Directory.CreateTempSubdirectory().FullName;
        _path = Path.Combine(_directory, "Library.xml");
        File.WriteAllText(_path, "<plist><dict><key>Version</key><string>original</string></dict></plist>");
        _reader = new ITunesXmlReader(_path);
        _writer = new ITunesXmlWriter(_path);
    }

    [TearDown]
    public void TearDown() => Directory.Delete(_directory, true);

    private static XDocument Doc(string value) =>
        XDocument.Parse($"<plist><dict><key>Version</key><string>{value}</string></dict></plist>");

    [Test]
    public async Task SaveXDocument_ReplacesTheFile()
    {
        await _writer.SaveXDocument(Doc("changed"), CancellationToken.None);

        (await _reader.GetXDocument(CancellationToken.None)).Descendants("string").Single().Value.Should().Be("changed");
    }

    [Test]
    public async Task SaveXDocument_KeepsTheOriginalAsABackup()
    {
        await _writer.SaveXDocument(Doc("first"), CancellationToken.None);
        await _writer.SaveXDocument(Doc("second"), CancellationToken.None);

        // the backup is of the original, not of the previous save
        XDocument.Load(_path + ".bak").Descendants("string").Single().Value.Should().Be("original");
        XDocument.Load(_path).Descendants("string").Single().Value.Should().Be("second");
    }

    [Test]
    public async Task SaveXDocument_LeavesNoTemporaryFile()
    {
        await _writer.SaveXDocument(Doc("changed"), CancellationToken.None);

        File.Exists(_path + ".tmp").Should().BeFalse();
    }

    [Test]
    public async Task SaveXDocument_WhenCancelled_LeavesTheLibraryUntouched()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        var act = () => _writer.SaveXDocument(Doc("changed"), cts.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
        XDocument.Load(_path).Descendants("string").Single().Value.Should().Be("original");
        File.Exists(_path + ".tmp").Should().BeFalse();
    }
}
