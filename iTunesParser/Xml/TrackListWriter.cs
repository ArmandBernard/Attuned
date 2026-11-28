using static iTunesSmartParser.Xml.PlistDictWriter;
using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public class TrackListWriter(ITrackListParser trackListParser) : ITrackListWriter
{
    public Track? ApplyPatch(XDocument doc, int id, TrackPatch patch)
    {
        var trackElement = PListParser.GetTopLevelDictionaryElement(doc)
            ?.PlistDictGet("Tracks")
            ?.PlistDictGet(id.ToString());

        if (trackElement == null)
        {
            return null;
        }

        SetString(trackElement, "Name", patch.Name);
        SetString(trackElement, "Artist", patch.Artist);
        SetString(trackElement, "Composer", patch.Composer);
        SetString(trackElement, "Album", patch.Album);
        SetString(trackElement, "Album Artist", patch.AlbumArtist);
        SetString(trackElement, "Genre", patch.Genre);
        SetInteger(trackElement, "Year", patch.Year);
        SetInteger(trackElement, "BPM", patch.Bpm);
        SetInteger(trackElement, "Disc Number", patch.DiscNumber);
        SetInteger(trackElement, "Disc Count", patch.DiscCount);
        SetInteger(trackElement, "Track Number", patch.TrackNumber);
        SetInteger(trackElement, "Track Count", patch.TrackCount);

        if (patch.Rating.HasValue)
        {
            // iTunes stores ratings out of 100, and has no key at all for unrated
            SetInteger(trackElement, "Rating", patch.Rating.Value == 0 ? null : patch.Rating.Value * 20);
        }

        if (patch.Loved.HasValue)
        {
            SetLoved(trackElement, patch.Loved.Value);
        }

        return trackListParser.ParseTrackElement(trackElement);
    }

    private static void SetString(XElement dict, string key, Option<string?> value)
    {
        if (value.HasValue)
        {
            SetValue(dict, key, value.Value == null ? null : new XElement("string", value.Value));
        }
    }

    private static void SetInteger(XElement dict, string key, Option<int?> value)
    {
        if (value.HasValue)
        {
            SetInteger(dict, key, value.Value);
        }
    }

    private static void SetInteger(XElement dict, string key, int? value) =>
        SetValue(dict, key, value == null ? null : new XElement("integer", value.Value));

    private static void SetLoved(XElement dict, bool? loved)
    {
        // "Loved" and "Disliked" are mutually exclusive flags. Neither being present means neither.
        SetValue(dict, "Loved", loved == true ? new XElement("true") : null);
        SetValue(dict, "Disliked", loved == false ? new XElement("true") : null);
    }
}
