using System.Xml.Linq;
using iTunesSmartParser.Xml;

namespace iTunesParserTests.Xml;

[TestFixture]
public class PlistParserTests
{
    // lang=XML
    private const string TestDocXml =
        """
        <?xml version="1.0" encoding="UTF-8"?>
        <!DOCTYPE plist PUBLIC "-//Apple Computer//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
        <plist version="1.0">
        <dict>
            <key>Major Version</key><integer>1</integer>
            <key>Tracks</key>
            <dict>
                <key>628</key>
                <dict>
                    <key>Track ID</key><integer>628</integer>
                    <key>Date Modified</key><date>2014-12-26T14:17:09Z</date>
                    <key>Album</key><string>Soul Eater</string>
                </dict>
            </dict>
            </dict>
        </plist>
        """;

    private readonly Dictionary<string, dynamic> _testDocDictionary = new()
    {
        {"Major Version", 1},
        {
            "Tracks", new Dictionary<string, dynamic>()
            {
                {
                    "628", new Dictionary<string, dynamic>()
                    {
                        {"Track ID", 628},
                        {"Date Modified", DateTime.Parse("2014-12-26T14:17:09Z")},
                        {"Album", "Soul Eater"}
                    }
                }
            }
        }
    };

    private readonly XDocument _testDoc = XDocument.Parse(TestDocXml);


    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ParseDocument_CanParseABasicDocument()
    {
        var parsedDoc = PListParser.ParseDocument(_testDoc);

        parsedDoc.Should().BeEquivalentTo(_testDocDictionary);
    }

    [Test]
    public void GetTopLevelDictionaryElement_GetsCorrectElement()
    {
        var element = PListParser.GetTopLevelDictionaryElement(_testDoc);

        element.Should().NotBeNull();
        element!.Name.LocalName.Should().Be("dict");
        element!.Parent!.Name.LocalName.Should().Be("plist");
    }

    [Test]
    public void PlistDictGet_GetsCorrectElement()
    {
        var element = PListParser.GetTopLevelDictionaryElement(_testDoc)?.PlistDictGet("Major Version");

        element.Should().NotBeNull();
        element!.Name.LocalName.Should().Be("integer");
        element.Value.Should().Be("1");
    }

    private static IEnumerable<TestCaseData> ParseValueElementSource()
    {
        yield return new TestCaseData("<string>Hello there!</string>", "Hello there!");
        yield return new TestCaseData("<integer>628</integer>", 628);
        yield return new TestCaseData("<date>2014-12-26T14:17:09Z</date>", DateTime.Parse("2014-12-26T14:17:09Z"));
        yield return new TestCaseData("""
                                      <data>AQEAAwAAAAIAAAAZAAAAAAAAAAcAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
                                      			AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
                                      			AAAAAA==</data>
                                      """,
            Convert.FromBase64String(
                "AQEAAwAAAAIAAAAZAAAAAAAAAAcAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=="));
        yield return new TestCaseData("<true/>", true);
        yield return new TestCaseData("<false/>", false);
        yield return new TestCaseData("""
                                      <dict>
                                        <key>Major Version</key><integer>1</integer>
                                        <key>Kind</key><string>MPEG audio file</string>
                                      </dict>
                                      """, new Dictionary<string, dynamic>()
        {
            {"Major Version", 1},
            {"Kind", "MPEG audio file"}
        });
    }

    [TestCaseSource(nameof(ParseValueElementSource))]
    public void ParseValueElement_ForGivenDataTypeParsesValueCorrectly(string stringXml, object expectedValue)
    {
        var element = XElement.Parse(stringXml);

        var value = PListParser.ParseValueElement(element);

        switch (value)
        {
            case Array array:
                array.Should().BeEquivalentTo(expectedValue);
                break;
            case Dictionary<string, dynamic> dictionary:
                dictionary.Should().BeEquivalentTo(expectedValue);
                break;
            default:
                ((object) value).Should().Be(expectedValue);
                break;
        }
    }
}