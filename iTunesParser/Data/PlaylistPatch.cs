using iTunesSmartParser.Data.Limits;

namespace iTunesSmartParser.Data;

/// <summary>
/// A partial update to a playlist. Only properties that have a value are changed, and a property that is not provided
/// is left as it was.
/// Everything except the name is about smart playlists, and can't be used on one that isn't.
/// </summary>
public record PlaylistPatch
{
    public Option<string> Name { get; init; }

    public Option<Limit> Limit { get; init; }

    public Option<bool> LiveUpdating { get; init; }

    /// <summary>
    /// The rules of the playlist. If this is provided as null, the playlist stops matching on its rules. The rules that
    /// were saved are kept, so that matching can be turned back on later without losing them.
    /// </summary>
    public Option<Conjunction?> Rules { get; init; }

    /// <summary>
    /// True if the patch would not change anything
    /// </summary>
    public bool IsEmpty => !(Name.HasValue || Limit.HasValue || LiveUpdating.HasValue || Rules.HasValue);

    /// <summary>
    /// True if the patch changes something that only a smart playlist has
    /// </summary>
    public bool ChangesSmartInformation => Limit.HasValue || LiveUpdating.HasValue || Rules.HasValue;
}
