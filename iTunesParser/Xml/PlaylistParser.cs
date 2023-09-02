using System.Xml.Linq;

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

        IEnumerable<Dictionary<string, dynamic>> playlistDictionaries = PListParser.ParseValueElement(playlistsArrayElement);

        return playlistDictionaries.Select(x => Playlist.FromDictionary(x));
    }
}