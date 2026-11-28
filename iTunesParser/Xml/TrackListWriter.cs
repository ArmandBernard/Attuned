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

    /// <summary>
    /// Set a key in a plist dict to the given value element, or remove the key if the value is null.
    /// </summary>
    private static void SetValue(XElement dict, string key, XElement? newValueElement)
    {
        var keyElement = dict.Elements("key").FirstOrDefault(x => x.Value == key);

        if (newValueElement == null)
        {
            if (keyElement == null)
            {
                return;
            }

            DeleteAtKey(keyElement);
        }
        else if (keyElement != null)
        {
            ReplaceAtKey(keyElement, newValueElement);
        }
        else
        {
            AddAfterLast(dict, key, newValueElement);
        }
    }
    
    private static void DeleteAtKey(XElement keyElement)
    {
        // the plist format is <key>[Name of property]</key><[type of property]>[value of property]</[type of property]>
        var valueElement = keyElement.NextElement();
        (keyElement.PreviousNode as XText)?.Remove(); // remove the whitespace before the key
        keyElement.Remove();
        valueElement?.Remove();
    }

    private static void ReplaceAtKey(XElement keyElement, XElement newValueElement)
    {
        keyElement.NextElement()!.ReplaceWith(newValueElement);
    }

    private static void AddAfterLast(XElement dict, string key, XElement newValueElement)
    {
        var last = dict.Elements().Last();

        // copy the indentation used by the existing entries
        var indentation = dict.Elements("key").First().PreviousNode as XText;

        if (indentation != null)
        {
            last.AddAfterSelf(new XText(indentation.Value), new XElement("key", key), newValueElement);
        }
        else
        {
            last.AddAfterSelf(new XElement("key", key), newValueElement);
        }
    }
    
}

