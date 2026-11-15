namespace iTunesSmartParser.Data;

/// <summary>
/// A partial update to a track. Only properties that have a value are changed. A property provided as null is
/// cleared, and a property that is not provided is left as it was.
/// </summary>
public record TrackPatch
{
    public Option<string?> Name { get; init; }
    public Option<string?> Artist { get; init; }
    public Option<string?> Composer { get; init; }
    public Option<string?> Album { get; init; }
    public Option<string?> AlbumArtist { get; init; }
    public Option<string?> Genre { get; init; }
    public Option<int?> Year { get; init; }
    public Option<int?> Bpm { get; init; }
    public Option<int?> DiscNumber { get; init; }
    public Option<int?> DiscCount { get; init; }
    public Option<int?> TrackNumber { get; init; }
    public Option<int?> TrackCount { get; init; }
    public Option<int> Rating { get; init; }
    public Option<bool?> Loved { get; init; }

    /// <summary>
    /// True if the patch would not change anything
    /// </summary>
    public bool IsEmpty =>
        !(Name.HasValue || Artist.HasValue || Composer.HasValue || Album.HasValue || AlbumArtist.HasValue ||
          Genre.HasValue || Year.HasValue || Bpm.HasValue || DiscNumber.HasValue || DiscCount.HasValue ||
          TrackNumber.HasValue || TrackCount.HasValue || Rating.HasValue || Loved.HasValue);
}
