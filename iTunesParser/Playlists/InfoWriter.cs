using iTunesSmartParser.Data;
using iTunesSmartParser.Data.Limits;

namespace iTunesSmartParser.Playlists;

/// <summary>
/// Changes the bytes iTunes stores as a smart playlist's "Smart Info". It is the inverse of <see cref="InfoHelper"/>.
///
/// The info is 112 bytes. The meaning of a few is known (see the offsets in <see cref="InfoHelper"/>). All of the
/// others are 0 in every playlist in a real library that was compared, except for two:
///
/// <list type="bullet">
///   <item>Byte 19 is always 7.</item>
///   <item>Byte 21 is 0 in some playlists and 1 in others. It doesn't match the playlist's rules, sorting, limit, or any
///   other value that is saved with the playlist, so nothing is known about what it means. It could be something
///   iTunes decided when the playlist was made.</item>
/// </list>
///
/// So that nothing iTunes put in the info is lost, these methods change a copy of the playlist's existing info. A
/// byte that isn't understood is always left as it was.
/// </summary>
public static class InfoWriter
{
    public const int LENGTH = 112;

    private const int ALWAYS_SEVEN = 19;

    /// <summary>
    /// The info for a playlist that has none to start from. Byte 21 is 0, which iTunes gives to about two thirds of
    /// playlists. Whether it matters is not known.
    /// </summary>
    public static byte[] NewInfo()
    {
        var info = new byte[LENGTH];
        info[ALWAYS_SEVEN] = 7;
        return info;
    }

    /// <summary>
    /// The info for a new playlist with the values in <paramref name="smartInfo"/>.
    /// </summary>
    public static byte[] Encode(SmartPlaylistInformation smartInfo) => Encode(NewInfo(), smartInfo);

    /// <summary>
    /// A copy of the info with all of the values in <paramref name="smartInfo"/> set.
    /// </summary>
    public static byte[] Encode(byte[] original, SmartPlaylistInformation smartInfo)
    {
        var info = SetLimit(original, smartInfo.Limit);
        info = SetLiveUpdating(info, smartInfo.LiveUpdating);
        return SetMatching(info, smartInfo.RuleConjunction != null);
    }

    /// <summary>
    /// A copy of the info with the limit set
    /// </summary>
    public static byte[] SetLimit(byte[] original, Limit limit)
    {
        if (!Enum.IsDefined(limit.Units))
        {
            throw new ArgumentException($"{limit.Units} is not a unit that a limit can be in.", nameof(limit));
        }

        if (!Enum.IsDefined(limit.SortBy))
        {
            throw new ArgumentException($"{limit.SortBy} is not a way that a limit can select songs.", nameof(limit));
        }

        if (limit.Amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(limit), limit.Amount, "A limit can't be below 0.");
        }

        var info = Copy(original);

        info[InfoHelper.LIMITBOOL] = ToByte(limit.IsLimited);
        info[InfoHelper.LIMITMETHOD] = (byte) limit.Units;
        Utils.IntToBytes(limit.Amount).CopyTo(info, InfoHelper.LIMITINT);
        info[InfoHelper.LIMITCHECKED] = ToByte(limit.OnlyChecked);
        info[InfoHelper.SELECTIONMETHOD] = (byte) limit.SortBy;
        // "most" is 0 and "least" is 1
        info[InfoHelper.SELECTIONMETHODSIGN] = ToByte(!limit.SortByDescending);

        return info;
    }

    /// <summary>
    /// A copy of the info with live updating turned on or off
    /// </summary>
    public static byte[] SetLiveUpdating(byte[] original, bool liveUpdating)
    {
        var info = Copy(original);
        info[InfoHelper.LIVEUPDATE] = ToByte(liveUpdating);
        return info;
    }

    /// <summary>
    /// A copy of the info with the playlist using its rules or not
    /// </summary>
    public static byte[] SetMatching(byte[] original, bool matching)
    {
        var info = Copy(original);
        info[InfoHelper.MATCHBOOL] = ToByte(matching);
        return info;
    }

    private static byte[] Copy(byte[] original)
    {
        if (original.Length != LENGTH)
        {
            throw new ArgumentException($"Smart info should be {LENGTH} bytes, but this is {original.Length}.",
                nameof(original));
        }

        return (byte[]) original.Clone();
    }

    private static byte ToByte(bool value) => value ? (byte) 1 : (byte) 0;
}
