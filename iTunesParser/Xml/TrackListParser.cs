using System.Xml.Linq;

namespace iTunesSmartParser.Xml;

public class TrackListParser : ITrackListParser
{
    public IEnumerable<Track> ParseTracks(XDocument doc)
    {
        // get the node which contains all tracks
        var tracksDictNode = PListParser.GetTopLevelDictionaryElement(doc)?.PlistDictGet("Tracks");

        if (tracksDictNode == null)
        {
            throw new Exception("Could not find the Tracks node");
        }

        var tracksDict = PListParser.ParseDictionary(tracksDictNode);

        return tracksDict.Values.Select(x => new Track(x));
    }
}