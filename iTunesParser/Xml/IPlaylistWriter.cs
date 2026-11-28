using System.Xml.Linq;
using iTunesSmartParser.Data;

namespace iTunesSmartParser.Xml;

public interface IPlaylistWriter
{
    /// <summary>
    /// Apply a patch to a playlist in the document, in place.
    /// If this throws, the document is unchanged.
    /// </summary>
    /// <returns>The playlist as it is after the patch, or null if there is no playlist with that id (the document is then unchanged)</returns>
    /// <exception cref="InvalidOperationException">The patch changes smart playlist information, but the playlist is not smart</exception>
    /// <exception cref="ArgumentException">The patch has rules or a limit that can't be stored in a smart playlist</exception>
    PlaylistDetails? ApplyPatch(XDocument doc, int id, PlaylistPatch patch);
}
