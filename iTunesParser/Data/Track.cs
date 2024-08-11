using System.Web;

namespace iTunesSmartParser.Data;

public record Track
{
    #region Properties

    /// <summary>
    /// The unique ID of this track
    /// </summary>
    public required int Id { get; init; }

    #region File

    public string MediaKind => "Music";

    /// <summary>
    /// The size of the track in bytes
    /// </summary>
    public int? Size { get; init; }

    /// <summary>
    /// The size of the track in MB (where 1kB = 1024 bytes etc.)
    /// </summary>
    public float? SizeMb => Size.HasValue ? Size / 1024F / 1024F : null;

    /// <summary>
    /// The file modify date
    /// </summary>
    public DateTime DateModified { get; init; }

    /// <summary>
    /// The date the file was added to iTunes
    /// </summary>
    public DateTime DateAdded { get; init; }

    /// <summary>
    /// The location on disk for this track
    /// </summary>
    public required string Location { get; init; }

    /// <summary>
    /// Get a filepath that windows will actually recognize
    /// </summary>
    public string? LocalLocation
    {
        get
        {
            // not a local location
            if (!Location.Contains("//localhost/"))
            {
                return null;
            }

            var locationWithUnixSlashes = HttpUtility
                // location is url encoded
                .UrlDecode(Location)
                // remove "file:" prefix
                .Replace("file:", "")
                // remove localhost prefix
                .Replace(@"//localhost/", "");

            // Replace forward slashes with environment-respecting delimiters
            return Path.Combine(locationWithUnixSlashes.Split("/"));
        }
    }

    #endregion

    #region Encoding

    public int? BitRate { get; init; }

    public int? SampleRate { get; init; }

    #endregion

    #region Track

    /// <summary>
    /// The total duration of the track
    /// </summary>
    public TimeSpan? TotalTime { get; init; }

    /// <summary>
    /// Year of release of the track
    /// </summary>
    public int? Year { get; init; }

    /// <summary>
    /// Tempo of the track in Beats Per Minute
    /// </summary>
    public int? Bpm { get; init; }

    /// <summary>
    /// The disc number this track comes from
    /// </summary>
    public int? DiscNumber { get; init; }

    /// <summary>
    /// The total number of discs in this album
    /// </summary>
    public int? DiscCount { get; init; }

    /// <summary>
    /// This tracks number in the album. For multi-disc albums, this is the number of track on
    /// that disc
    /// </summary>
    public int? TrackNumber { get; init; }

    /// <summary>
    /// Total Tracks in this tracks containing album. For multi-disc albums, this is the number
    /// of tracks on that disc
    /// </summary>
    public int? TrackCount { get; init; }

    /// <summary>
    /// The Track's name / title
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// The Track's Artist(s) (usually semicolon delimited)
    /// </summary>
    public string? Artist { get; init; }

    /// <summary>
    /// The Track's Composer
    /// </summary>
    public string? Composer { get; init; }

    /// <summary>
    /// The album this track belongs to
    /// </summary>
    public string? Album { get; init; }

    /// <summary>
    /// The genre of music this track best fits
    /// </summary>
    public string? Genre { get; init; }

    #endregion

    #region Play

    /// <summary>
    /// Times played.
    /// </summary>
    public int? PlayCount { get; init; }

    /// <summary>
    /// Last played date
    /// </summary>
    public DateTime? PlayDate { get; init; }

    /// <summary>
    /// The number of times as this track has been skipped
    /// </summary>
    public int? SkipCount { get; init; }

    #endregion

    #region Rating

    /// <summary>
    /// Track Rating out of 5. 0 is unrated.
    /// </summary>
    public int Rating { get; init; }

    /// <summary>
    /// Has this track been marked as "loved" (true) "disliked" (false) or neither?
    /// </summary>
    public bool? Loved { get; init; }

    #endregion

    #endregion
}