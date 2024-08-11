using System.Xml;
using System.Xml.Linq;
using iTunesSmartParser.Data;
using iTunesSmartParser.Data.Limits;
using iTunesSmartParser.Playlists;

namespace iTunesSmartParser.Xml;

public class PlaylistParser : IPlaylistParser
{
    public IEnumerable<Playlist> ParseDocument(XDocument doc) =>
        GetPlaylistsArrayNode(doc).Elements().Select(ParsePlaylist);

    public PlaylistDetails? GetById(XDocument doc, int id)
    {
        var playlistElements = GetPlaylistsArrayNode(doc).Elements();
        
        var playlistNode = playlistElements
            .FirstOrDefault(playlist => playlist.PlistDictGet("Playlist ID")?.PlistParseValue() == id);

        return playlistNode == null ? null : ParsePlaylistDetails(playlistNode);
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

    private static Playlist ParsePlaylist(XElement playlistElement)
    {
        var dictionary = PListParser.ParseDictionary(playlistElement);

        return GetBasicFields(dictionary);
    }
    
    public PlaylistDetails ParsePlaylistDetails(XElement playlistElement)
    {
        var dictionary = PListParser.ParseDictionary(playlistElement);

        var basicFields = GetBasicFields(dictionary);

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
        var playlistItemsDictionaries =
            ((IEnumerable<object>) dictionary.GetValueOrDefault("Playlist Items",
                new List<Dictionary<string, dynamic>>())).Cast<Dictionary<string, dynamic>>();
        
        var playlistItems = playlistItemsDictionaries.Select(x => (int) x.Values.Single());
        
        SmartPlaylistInformation? playlistInfo = null;

        if (!basicFields.IsSmart)
        {
            return new PlaylistDetails(basicFields.Name, basicFields.Id, basicFields.IsSmart, playlistItems, playlistInfo);
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
            throw new XmlException($"Failed to parse playlist '{basicFields.Name}'. Exception:\n{ex}");
        }

        return new PlaylistDetails(basicFields.Name, basicFields.Id, basicFields.IsSmart, playlistItems, playlistInfo);
    }

    private static Playlist GetBasicFields(Dictionary<string,dynamic> playlistDictionary)
    {
        var name = playlistDictionary["Name"];

        var id = (int) playlistDictionary["Playlist ID"];
        
        var isSmart = playlistDictionary.ContainsKey("Smart Info") && playlistDictionary.ContainsKey("Smart Criteria");

        return new Playlist(name, id, isSmart);
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