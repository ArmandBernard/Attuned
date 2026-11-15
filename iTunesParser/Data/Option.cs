namespace iTunesSmartParser.Data;

/// <summary>
/// A value that may or may not have been provided.
/// </summary>
public readonly struct Option<T>
{
    private Option(T value)
    {
        Value = value;
        HasValue = true;
    }

    public bool HasValue { get; }

    public T Value => HasValue ? field : throw new InvalidOperationException("No value was provided.");

    public static Option<T> Of(T value) => new(value);

    public static implicit operator Option<T>(T value) => Of(value);
}
