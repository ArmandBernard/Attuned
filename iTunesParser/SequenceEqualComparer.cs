namespace iTunesSmartParser;

public class SequenceEqualComparer<T> : IEqualityComparer<IEnumerable<T>>
{
    public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
    {
        return ReferenceEquals(x, y) || (x != null && y != null && x.SequenceEqual(y));
    }

    public int GetHashCode(IEnumerable<T> obj)
    {
        unchecked // avoid stack overflow exception
        {
            return obj.Where(e => e != null).Select(e => e!.GetHashCode()).Aggregate(17, (a, b) => 23 * a + b);
        }
    }
}