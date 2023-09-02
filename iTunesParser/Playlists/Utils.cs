namespace iTunesSmartParser.Playlists;

public static class Utils
{
    const int UNIXDELTA = -2082844800; // iTunes/Unix time stamp 0 difference

    /// <summary>
    /// Convert bytes to integer.
    /// iTunes uses Big Endian encoding for its integers (reversed bytes), so bitconverter needs the array reversed
    /// if the converter itself is little endian (usually yes)
    /// </summary>
    /// <returns></returns>
    public static int ByteToInt(IEnumerable<byte> bytes) =>
        BitConverter.ToInt32(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes.ToArray(), 0);

    public static DateTime BytesToDateTime(IEnumerable<byte> bytes) => UnixToDateTime(ByteToUnix(bytes));
    
    private static long ByteToUnix(IEnumerable<byte> byteArray)
    {
        return ByteToInt(byteArray) + UNIXDELTA;
    }

    /// <summary>
    /// Convert Unix time value to a DateTime object.
    /// </summary>
    private static DateTime UnixToDateTime(long unixtime) => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
        .AddMilliseconds(unixtime).ToLocalTime();
}