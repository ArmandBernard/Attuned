using System.Buffers.Binary;

namespace iTunesSmartParser;

public static class Utils
{
    /// <summary>
    /// Dates in smart playlist rules are a number of seconds since this moment, as classic Mac OS counted them
    /// </summary>
    private static readonly DateTime MacEpoch = new(1904, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Convert bytes to integer.
    /// iTunes uses Big Endian encoding for its integers (reversed bytes), so bitconverter needs the array reversed
    /// if the converter itself is little endian (usually yes)
    /// </summary>
    /// <returns></returns>
    public static int ByteToInt(IEnumerable<byte> bytes) =>
        BitConverter.ToInt32(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes.ToArray(), 0);

    /// <summary>
    /// Convert an integer to the 4 big endian bytes iTunes uses. The inverse of <see cref="ByteToInt"/>.
    /// </summary>
    public static byte[] IntToBytes(int value)
    {
        var bytes = new byte[4];
        BinaryPrimitives.WriteInt32BigEndian(bytes, value);
        return bytes;
    }

    /// <summary>
    /// Convert a date stored as 4 big endian bytes to a date. The bytes are an unsigned number of seconds since 1904.
    /// </summary>
    public static DateTime BytesToDateTime(IEnumerable<byte> bytes) =>
        MacEpoch.AddSeconds(BinaryPrimitives.ReadUInt32BigEndian(bytes.ToArray()));

    /// <summary>
    /// The inverse of <see cref="BytesToDateTime"/>. Dates before 1904 or after 2040 can't be stored.
    /// </summary>
    public static uint DateTimeToMacSeconds(DateTime dateTime)
    {
        var seconds = Math.Floor((dateTime.ToUniversalTime() - MacEpoch).TotalSeconds);

        if (seconds is < 0 or > uint.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(dateTime), dateTime,
                "Smart playlist dates must be between 1904 and 2040.");
        }

        return (uint) seconds;
    }
}
