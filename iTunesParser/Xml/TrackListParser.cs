using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public class TrackListParser : ITrackListParser
{
    public IEnumerable<Track> ParseTracks(XDocument doc)
    {
        // get the node which contains all tracks
        var tracksDictNode = PListParser.GetTopLevelDictionaryElement(doc)?.PlistDictGet("Tracks");

        if (tracksDictNode == null)
        {
            throw new Exception("Could not find the Tracks node");
        }

        return tracksDictNode.PlistDictKeys().Select(ParseTrackElement);
    }

    public Track ParseTrackElement(XElement playlistElement)
    {
        Dictionary<string, dynamic?> properties = PListParser.ParseDictionary(playlistElement)!;

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
            Loved = properties.GetValueOrDefault("Loved", false),
            Name = properties.GetValueOrDefault("Name", null),
            Artist = properties.GetValueOrDefault("Artist", null),
            Composer = properties.GetValueOrDefault("Composer", null),
            Album = properties.GetValueOrDefault("Album", null),
            Genre = properties.GetValueOrDefault("Genre", null),
            Location = properties["Location"]!,
        };
    }
    
    // This is temporarily disabled while the XML-sourced information is worked on.
    public void LoadTagInfo(Track track)
    {
        if (track.LocalLocation != null && !File.Exists(track.LocalLocation))
        {
            return;
        }

        using var file = TagLib.File.Create(track.LocalLocation);
        
        track.Type = file.MimeType.Replace("taglib/", "");
        track.Codec = file.Properties.Description;
        track.Channels = file.Properties.AudioChannels;
    }
    
    // This is temporarily disabled while the XML-sourced information is worked on.
    public void LoadImage(Track track)
    {
        if (track.LocalLocation != null && !File.Exists(track.LocalLocation))
        {
            return;
        }
        
        using var file = TagLib.File.Create(track.LocalLocation);
        
        // if there are any pictures
        if (file.Tag.Pictures.Length >= 1)
        {
            track.CoverArt = file.Tag.Pictures[0].Data.Data;
        }
    }
}