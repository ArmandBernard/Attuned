using System.Xml;
using iTunesSmartParser.Playlists;

namespace iTunesSmartParser.Data;

public record Playlist(string Name, int Id, IEnumerable<int> Items, bool IsSmart, PlaylistInformation? Filters)
{
    public static Playlist FromDictionary(Dictionary<string, dynamic> properties)
    {
        var name = properties["Name"];

        var id = properties["Playlist ID"];

        //properties.TryGetValue("Playlist Items", out var playlistItemsDictionaries);

        var playlistItemsDictionaries =
            ((IEnumerable<object>) properties.GetValueOrDefault("Playlist Items",
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

        var isSmart = properties.ContainsKey("Smart Info") && properties.ContainsKey("Smart Criteria");

        PlaylistInformation? playlistInfo = null;
        
        if (isSmart)
        {
            try
            {
                // try to parse playlist info
                playlistInfo = PlaylistInformationBuilder.ParsePlaylist(
                    (byte[])properties["Smart Info"], (byte[])properties["Smart Criteria"]
                );
            }
            catch (Exception ex)
            {
                throw new XmlException($"Failed to parse playlist '{name}'. Exception:\n{ex}");
            }
        }

        return new Playlist(name, (int) id, playlistItems, isSmart, playlistInfo);
    }
}