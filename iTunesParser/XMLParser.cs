using System.Xml.Linq;

namespace iTunesSmartParser;

public class iTunesXMLParser
{
    public iTunesXMLParser(ILogger logger)
    {
        Logger = logger;
    }

    private ILogger Logger { get; init; }

    #region Tracks
    /// <summary>
    /// Load all tracks from the iTunes library XML
    /// </summary>
    /// <param name="path">The path to the XML</param>
    /// <returns></returns>
    public static List<Track> LoadTracks(string path)
    {
        return LoadTracksAsync(path).Result;
    }

    /// <summary>
    /// Load all tracks from the iTunes library XML using the XDoc method
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Task<List<Track>> LoadTracksAsync(string path)
    {
        var doc = XDocument.Load(path);

        // get the node which contains all tracks
        var tracksNode = (XElement?)doc
            ?.Element("plist")
            ?.Element("dict")
            ?.Elements()
            .First(e => e.Value == "Tracks")
            .NextNode;

        if (tracksNode == null)
        {
            throw new Exception("Could not find the Playlists node");
        }

        // get a list of track dict elements
        var trackElements = tracksNode.Elements().Where(e => e.Name == "dict").ToList();

        // create reporting object
        var progressTracker = new Progress<int>();

        var task = Task<List<Track>>.Factory.StartNew(
            () => LoadTracks(trackElements, progressTracker)
            );

        return task;
    }

    private static List<Track> LoadTracks(List<XElement> xElements, IProgress<int> progress)
    {
        var tracks = new List<Track>();

        // Cycle through tracks and parse all properties.
        // The property nodes follow the following pattern:
        //  <key>Track ID</key><integer>640</integer>
        //  <key>Size</key><integer>10774278</integer>
        //  ...
        for (int i = 0; i < xElements.Count; i++)
        {
            // report progress on the task if the functionality exists
            progress?.Report(i);

            tracks.Add(
                new Track(
                    // cycle through properties and create a properties dictionary (name : value)
                    xElements[i].ToPropertyDictionary()
                    )
            );
        }

        // report progress on the task
        progress?.Report(xElements.Count);

        return tracks;
    }

    #endregion

    public List<Playlist> LoadSmartPlaylists(string path, IProgress<int>? progress = null)
    {
        var doc = XDocument.Load(path);

        // get the node which contains all playlists
        var plNode = (XElement?)doc
            ?.Element("plist")
            ?.Element("dict")
            ?.Elements()
            .First(e => e.Value == "Playlists")
            .NextNode;

        if (plNode == null)
        {
            throw new Exception("Could not find the Playlists node");
        }

        // get a list of playlist dict elements
        var plElements = plNode.Elements().Where(e => e.Name == "dict").ToList();

        return LoadSmartPlaylists(plElements, progress);
    }

    private List<Playlist> LoadSmartPlaylists(List<XElement> xElements, IProgress<int>? progress)
    {
        var playlists = new List<Playlist>();

        // Cycle through tracks and parse all properties.
        // The property nodes follow the following pattern:
        //  <key>Track ID</key><integer>640</integer>
        //  <key>Size</key><integer>10774278</integer>
        //  ...
        for (int i = 0; i < xElements.Count; i++)
        {
            // report progress on the task if the functionality exists
            progress?.Report(i);

            var properties = xElements[i].ToPropertyDictionary();

            var name = properties["Name"];

            var id = Convert.ToInt32(properties["Playlist ID"]);

            var isSmart = properties.ContainsKey("Smart Info") && properties.ContainsKey("Smart Criteria");

            Parser.PlayListInfo? playlistInfo = null;

            var items = new List<int>();

            if (isSmart)
            {
                try
                {
                    // try to parse playlist info
                    playlistInfo = Parser.ParsePlaylistInfo(
                        properties["Smart Info"], properties["Smart Criteria"], true, false
                        );
                }
                catch (Exception ex)
                {
                    Logger.Log($"Failed to parse playlist '{name}'.");
                    Logger.Log(ex);
                }
            }

            // if there is an item list
            if (properties.ContainsKey("Playlist Items"))
            {
                // parse playlist items
                var xDoc = XDocument.Parse(properties["Playlist Items"]);

                // get all track IDs
                items = xDoc.Descendants()
                    .Where(x => x.Value == "Track ID")
                    .Select(x => Convert.ToInt32(((XElement)x.NextNode!).Value))
                    .ToList();
            }

            playlists.Add(new Playlist(name, id, isSmart, items, playlistInfo?.QueryWhere));
        }

        return playlists;
    }


}
