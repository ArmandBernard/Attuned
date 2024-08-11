using System.Xml;
using System.Xml.Linq;
using iTunesSmartParser.Data;
using iTunesSmartParser.Data.Limits;
using iTunesSmartParser.Playlists;

namespace iTunesSmartParser.Xml;

public class PlaylistParser : IPlaylistParser
{
    public IEnumerable<Playlist> ParseDocument(XDocument doc)
    {
        // get the node which contains all tracks
        var playlistsArrayElement = PListParser.GetTopLevelDictionaryElement(doc)?.PlistDictGet("Playlists");

        if (playlistsArrayElement == null)
        {
            throw new Exception("Could not find the Playlists node");
        }

        return playlistsArrayElement.Elements().Select(ParsePlaylistElement);
    }
    
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
        
        if (isSmart)
        {
            try
            {
                // try to parse playlist info
                playlistInfo = ParseSmartInformation(
                    (byte[])dictionary["Smart Info"], (byte[])dictionary["Smart Criteria"]
                );
            }
            catch (Exception ex)
            {
                throw new XmlException($"Failed to parse playlist '{name}'. Exception:\n{ex}");
            }
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