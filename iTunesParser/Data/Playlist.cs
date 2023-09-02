using System.Xml;

namespace iTunesSmartParser;

public record Playlist(string Name, int Id, IEnumerable<int> Items, bool IsSmart, string? Filter)
{
    public static Playlist FromDictionary(Dictionary<string, dynamic> properties)
    {
        var name = properties["Name"];

        var id = properties["Playlist ID"];

        var isSmart = properties.ContainsKey("Smart Info") && properties.ContainsKey("Smart Criteria");

        IEnumerable<Dictionary<string, int>> playlistItemsDictionaries = properties.GetValueOrDefault("Playlist Items", new List<Dictionary<string, dynamic>>());

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
        var playlistItems = playlistItemsDictionaries.Select(x => x.Values.First());

        string? filter = null;

        if (isSmart)
        {
            try
            {
                // try to parse playlist info
                var playlistInfo = Parser.ParsePlaylistInfo(
                    properties["Smart Info"], properties["Smart Criteria"], true, false
                    );

                filter = playlistInfo?.QueryWhere;
            }
            catch (Exception ex)
            {
                throw new XmlException($"Failed to parse playlist '{name}'. Exception:\n{ex}");
            }
        }

        return new Playlist(name, id, playlistItems, isSmart, filter);
    }
}
