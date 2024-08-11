using System.Xml;
using System.Xml.Linq;
using iTunesSmartParser.Data;
using iTunesSmartParser.Data.Limits;
using iTunesSmartParser.Playlists;

namespace iTunesSmartParser.Xml;

public class PlaylistParser : IPlaylistParser
{
    public IEnumerable<Playlist> ParseDocument(XDocument doc) =>
        GetPlaylistsArrayNode(doc).Elements().Select(ParsePlaylistElement);

    public Playlist? GetById(XDocument doc, int id)
    {
        var playlistElements = GetPlaylistsArrayNode(doc).Elements();
        
        var playlistNode = playlistElements
            .FirstOrDefault(playlist => playlist.PlistDictGet("Playlist ID")?.PlistParseValue() == id);

        return playlistNode == null ? null : ParsePlaylistElement(playlistNode);
    }

    /// <summary>
    /// Get the node which contains all playlists
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static XElement GetPlaylistsArrayNode(XDocument doc) =>
        PListParser.GetTopLevelDictionaryElement(doc)?.PlistDictGet("Playlists") ??
        throw new Exception("Could not find the Playlists node");

    public Playlist ParsePlaylistElement(XElement playlistElement)
    {
        var dictionary = PListParser.ParseDictionary(playlistElement);

        var name = dictionary["Name"];

        var id = dictionary["Playlist ID"];

        //properties.TryGetValue("Playlist Items", out var playlistItemsDictionaries);

        var playlistItemsDictionaries =
            ((IEnumerable<object>) dictionary.GetValueOrDefault("Playlist Items",
                new List<Dictionary<string, dynamic>>())).Cast<Dictionary<string, dynamic>>();

        // The structure of playlistItems here is:
        // <array>
        //   <dict>
        //	   <key>Track ID</key><integer>764</integer>
        //   </dict>
        //   <dict>
        //     <key>Track ID</key><integer>2982</integer>
        //   </dict>
        //   ...
        // </array>
        var playlistItems = playlistItemsDictionaries.Select(x => (int) x.Values.Single());

        var isSmart = dictionary.ContainsKey("Smart Info") && dictionary.ContainsKey("Smart Criteria");

        SmartPlaylistInformation? playlistInfo = null;

        if (!isSmart)
        {
            return new Playlist(name, (int) id, playlistItems, isSmart, playlistInfo);
        }

        try
        {
            // try to parse playlist info
            playlistInfo = ParseSmartInformation(
                (byte[]) dictionary["Smart Info"], (byte[]) dictionary["Smart Criteria"]
            );
        }
        catch (Exception ex)
        {
            throw new XmlException($"Failed to parse playlist '{name}'. Exception:\n{ex}");
        }

        return new Playlist(name, (int) id, playlistItems, isSmart, playlistInfo);
    }

    private static SmartPlaylistInformation ParseSmartInformation(byte[] info, byte[] criteria)
    {
        var infoHelper = new InfoHelper(info);

        var limit = new Limit(infoHelper.IsLimited, infoHelper.LimitUnits,
            infoHelper.LimitNumber, infoHelper.OnlyChecked, infoHelper.SortBy,
            infoHelper.SortDescending);

        // perform logical matching
        var conjunctions = infoHelper.Match ? CriteriaParser.Parse(criteria) : null;

        return new SmartPlaylistInformation(limit, conjunctions, infoHelper.LiveUpdate);
    }
}