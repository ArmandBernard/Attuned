using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public class TrackListParser : ITrackListParser
{
    public IEnumerable<Track> ParseDocument(XDocument doc) =>
        GetTracksNode(doc).PlistDictKeys().Select(ParseTrackElement);

    public TrackDetails? GetById(XDocument doc, int id)
    {
        var trackNode = GetTracksNode(doc).PlistDictGet(id.ToString());

        if (trackNode == null)
        {
            return null;
        }

        var track = ParseTrackElement(trackNode);

        return LoadImage(LoadTagInfo(new TrackDetails(track)));
    }

    public Dictionary<byte[], HashSet<int>> GetAllImages(XDocument doc, Func<byte[], byte[]>? processImage = null)
    {
        var deduplicator = new Dictionary<byte[], HashSet<int>>(new SequenceEqualComparer<byte>());

        foreach (var trackNode in GetTracksNode(doc).PlistDictKeys())
        {
            var track = ParseTrackElement(trackNode);

            var image = GetImage(track.LocalLocation);

            if (image == null)
            {
                continue;
            }

            if (processImage != null)
            {
                try
                {
                    image = processImage(image);
                }
                catch
                {
                }
            }

            if (deduplicator.TryGetValue(image, out var value))
            {
                value.Add(track.Id);
            }
            else
            {
                deduplicator.Add(image, new HashSet<int>(track.Id));
            }
        }

        return deduplicator;
    }

    /// <summary>
    /// Get the node which contains all tracks
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static XElement GetTracksNode(XDocument doc) =>
        PListParser.GetTopLevelDictionaryElement(doc)?.PlistDictGet("Tracks") ??
        throw new Exception("Could not find the Tracks node");

    public Track ParseTrackElement(XElement tracksElement)
    {
        Dictionary<string, dynamic?> properties = PListParser.ParseDictionary(tracksElement)!;

        var loved = properties.GetValueOrDefault("Loved", false);
        var disliked = properties.GetValueOrDefault("Disliked", false);

        return new Track
        {
            // parse all field
            Id = (int) properties["Track ID"]!,
            Size = (int?) properties.GetValueOrDefault("Size", null),
            TotalTime = properties.TryGetValue("Total Time", out var property)
                ? TimeSpan.FromMilliseconds(property)
                : null,
            Year = (int?) properties.GetValueOrDefault("Year", null),
            Bpm = (int?) properties.GetValueOrDefault("BPM", null),
            DiscNumber = (int?) properties.GetValueOrDefault("Disc Number", null),
            DiscCount = (int?) properties.GetValueOrDefault("Disc Count", null),
            TrackNumber = (int?) properties.GetValueOrDefault("Track Number", null),
            TrackCount = (int?) properties.GetValueOrDefault("Track Count", null),
            DateModified = properties["Date Modified"],
            DateAdded = properties["Date Added"],
            BitRate = (int?) properties.GetValueOrDefault("Bit Rate", null),
            SampleRate = (int?) properties.GetValueOrDefault("Sample Rate", null),
            PlayCount = (int?) properties.GetValueOrDefault("Play Count", null),
            PlayDate = properties.GetValueOrDefault("Play Date UTC", null),
            SkipCount = (int?) properties.GetValueOrDefault("Skip Count", null),
            Rating = (int) (properties.GetValueOrDefault("Rating", 0) / 20),
            Loved = loved ? true : (disliked ? false : null),
            Name = properties.GetValueOrDefault("Name", null),
            Artist = properties.GetValueOrDefault("Artist", null),
            Composer = properties.GetValueOrDefault("Composer", null),
            Album = properties.GetValueOrDefault("Album", null),
            Genre = properties.GetValueOrDefault("Genre", null),
            Location = properties["Location"]!,
        };
    }

    // This is temporarily disabled while the XML-sourced information is worked on.
    private static TrackDetails LoadTagInfo(TrackDetails track)
    {
        if (track.LocalLocation != null && !File.Exists(track.LocalLocation))
        {
            return track;
        }

        using var file = TagLib.File.Create(track.LocalLocation);

        return track with
        {
            Type = file.MimeType.Replace("taglib/", ""),
            Codec = file.Properties.Description,
            Channels = file.Properties.AudioChannels,
        };
    }

    private static TrackDetails LoadImage(TrackDetails track)
    {
        var coverArt = GetImage(track.LocalLocation);

        return coverArt != null
            ? track with
            {
                CoverArt = coverArt
            }
            : track;
    }

    private static byte[]? GetImage(string? path)
    {
        if (path == null || !File.Exists(path))
        {
            return null;
        }

        using var file = TagLib.File.Create(path);

        var pictures = file.Tag.Pictures;

        // if there are any pictures
        return pictures.Length >= 1 ? pictures[0].Data.Data : null;
    }
}