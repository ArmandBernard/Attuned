using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public class PlaylistParser : IPlaylistsParser
{
    public IEnumerable<Playlist> ParsePlaylists(XDocument doc)
    {
        // get the node which contains all tracks
        var playlistsArrayElement = PListParser.GetTopLevelDictionaryElement(doc)?.PlistDictGet("Playlists");

        if (playlistsArrayElement == null)
        {
            throw new Exception("Could not find the Playlists node");
        }

        var playlistDictionaries =
            ((IEnumerable<object>) PListParser.ParseValueElement(playlistsArrayElement))
            .Cast<Dictionary<string, dynamic>>();

        return playlistDictionaries.Select(x => Playlist.FromDictionary(x));
    }
}