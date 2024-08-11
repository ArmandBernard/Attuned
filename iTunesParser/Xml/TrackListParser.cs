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

    // This is temporarily disabled while the XML-sourced information is worked on.
    private static TrackDetails LoadImage(TrackDetails track)
    {
        if (track.LocalLocation != null && !File.Exists(track.LocalLocation))
        {
            return track;
        }

        using var file = TagLib.File.Create(track.LocalLocation);

        // if there are any pictures
        if (file.Tag.Pictures.Length >= 1)
        {
            return track with
            {
                CoverArt = file.Tag.Pictures[0].Data.Data
            };
        }

        return track;
    }
}