namespace iTunesSmartParser;

internal static class Extensions
{
    /// <summary>
    /// Get sub array from array using index + length
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="index"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static T[] SubArrayL<T>(this T[] data, int index, int length)
    {
        T[] result = new T[length];
        Array.Copy(data, index, result, 0, length);
        return result;
    }

    /// <summary>
    /// Get sub array from array using index + length
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="index"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static T[] SubArrayR<T>(this T[] data, int start, int end)
    {
        T[] result = new T[end - start];
        Array.Copy(data, start, result, 0, end - start);
        return result;
    }
}
