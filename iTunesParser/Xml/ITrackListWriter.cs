using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public interface ITrackListWriter
{
    /// <summary>
    /// Apply a patch to a track in the document, in place.
    /// </summary>
    /// <returns>The track as it is after the patch, or null if there is no track with that id (the document is then unchanged)</returns>
    Track? ApplyPatch(XDocument doc, int id, TrackPatch patch);
}
