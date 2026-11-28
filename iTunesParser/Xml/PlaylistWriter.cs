using System.Xml.Linq;
using iTunesSmartParser.Data;
using iTunesSmartParser.Playlists;
using static iTunesSmartParser.Xml.PlistDictWriter;

namespace iTunesSmartParser.Xml;

public class PlaylistWriter(IPlaylistParser playlistParser) : IPlaylistWriter
{
    private const string SMART_INFO_KEY = "Smart Info";
    private const string SMART_CRITERIA_KEY = "Smart Criteria";

    public PlaylistDetails? ApplyPatch(XDocument doc, int id, PlaylistPatch patch)
    {
        var playlistElement = PListParser.GetTopLevelDictionaryElement(doc)
            ?.PlistDictGet("Playlists")
            ?.Elements()
            .FirstOrDefault(x => x.PlistDictGet("Playlist ID")?.Value == id.ToString());

        if (playlistElement == null)
        {
            return null;
        }

        // Work out everything that will be written before writing any of it, so that a patch that can't be applied
        // doesn't change half of the playlist.
        var smartInfo = patch.ChangesSmartInformation ? EncodeSmartInformation(playlistElement, patch) : null;

        if (patch.Name.HasValue)
        {
            SetValue(playlistElement, "Name", new XElement("string", patch.Name.Value));
        }

        if (smartInfo != null)
        {
            SetData(playlistElement, SMART_INFO_KEY, smartInfo.Info);

            if (smartInfo.Criteria != null)
            {
                SetData(playlistElement, SMART_CRITERIA_KEY, smartInfo.Criteria);
            }
        }

        return playlistParser.ParsePlaylistDetails(playlistElement);
    }

    private record SmartInformationBytes(byte[] Info, byte[]? Criteria);

    private static SmartInformationBytes EncodeSmartInformation(XElement playlistElement, PlaylistPatch patch)
    {
        var infoElement = playlistElement.PlistDictGet(SMART_INFO_KEY);
        var criteriaElement = playlistElement.PlistDictGet(SMART_CRITERIA_KEY);

        if (infoElement == null || criteriaElement == null)
        {
            throw new InvalidOperationException("Only a smart playlist can have a limit, rules or live updating.");
        }

        // Change the info the playlist has. This way anything that iTunes saved in it that isn't understood is kept.
        var info = (byte[]) infoElement.PlistParseValue();

        if (patch.Limit.HasValue)
        {
            info = InfoWriter.SetLimit(info, patch.Limit.Value);
        }

        if (patch.LiveUpdating.HasValue)
        {
            info = InfoWriter.SetLiveUpdating(info, patch.LiveUpdating.Value);
        }

        byte[]? criteria = null;

        if (patch.Rules.HasValue)
        {
            var rules = patch.Rules.Value;

            // Without rules, the criteria that is already there is left alone. iTunes saves criteria in a playlist even
            // when it isn't using them.
            // The existing criteria is given so that rules stay in the order the person has them in.
            criteria = rules == null ? null : CriteriaEncoder.Encode(rules, (byte[]) criteriaElement.PlistParseValue());

            info = InfoWriter.SetMatching(info, rules != null);
        }

        return new SmartInformationBytes(info, criteria);
    }

    /// <summary>
    /// Set the content of a data element, keeping the layout of the text that was there
    /// </summary>
    private static void SetData(XElement playlistElement, string key, byte[] bytes)
    {
        var dataElement = playlistElement.PlistDictGet(key)!;

        dataElement.Value = PlistDataFormatter.Format(bytes, dataElement.Value);
    }
}
