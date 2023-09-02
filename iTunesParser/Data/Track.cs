using TagFile = TagLib.File;

namespace iTunesSmartParser.Data;

public class Track
{
    #region Properties

    /// <summary>
    /// The unique ID of this track
    /// </summary>
    public int Id { get; set; }

    #region File

    public string MediaKind => "Music";

    /// <summary>
    /// The size of the track in bytes
    /// </summary>
    public int? Size { get; set; }

    /// <summary>
    /// The size of the track in MB (where 1kB = 1024 bytes etc.)
    /// </summary>
    public float? SizeMb => Size.HasValue ? Size / 1024F / 1024F : null;

    /// <summary>
    /// The file modify date
    /// </summary>
    public DateTime DateModified { get; set; }

    /// <summary>
    /// The date the file was added to iTunes
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// The location on disk for this track
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// Get a filepath that windows will actually recognize
    /// </summary>
    public string WellFormattedLocation
    {
        get
        {
            string reformatted = Location.Replace("/", "\\");
            // remove "file:" prefix
            reformatted = reformatted.Replace("file:", "");
            // spaces are denoted by %20 in Apple's filepaths
            reformatted = reformatted.Replace("%20", " ");
            // remove localhost prefix
            reformatted = reformatted.Replace("\\\\localhost\\", "");
            return reformatted;
        }
    }

    #endregion

    #region From Tags

    public int? Channels { get; set; }

    public string? Type { get; set; }

    public string? Codec { get; set; }

    public byte[]? CoverArt;

    #endregion

    #region Encoding

    public int? BitRate { get; set; }

    public int? SampleRate { get; set; }

    #endregion

    #region Track

    /// <summary>
    /// The total duration of the track
    /// </summary>
    public TimeSpan? TotalTime { get; set; }

    /// <summary>
    /// Year of release of the track
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// Tempo of the track in Beats Per Minute
    /// </summary>
    public int? Bpm { get; set; }

    /// <summary>
    /// The disc number this track comes from
    /// </summary>
    public int? DiscNumber { get; set; }

    /// <summary>
    /// The total number of discs in this album
    /// </summary>
    public int? DiscCount { get; set; }

    /// <summary>
    /// This tracks number in the album. For multi-disc albums, this is the number of track on
    /// that disc
    /// </summary>
    public int? TrackNumber { get; set; }

    /// <summary>
    /// Total Tracks in this tracks containing album. For multi-disc albums, this is the number
    /// of tracks on that disc
    /// </summary>
    public int? TrackCount { get; set; }

    /// <summary>
    /// The Track's name / title
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The Track's Artist(s) (usually semicolon delimited)
    /// </summary>
    public string? Artist { get; set; }

    /// <summary>
    /// The Track's Composer
    /// </summary>
    public string? Composer { get; set; }

    /// <summary>
    /// The album this track belongs to
    /// </summary>
    public string? Album { get; set; }

    /// <summary>
    /// The genre of music this track best fits
    /// </summary>
    public string? Genre { get; set; }



    #endregion

    #region Play

    /// <summary>
    /// Times played.
    /// </summary>
    public int? PlayCount { get; set; }

    /// <summary>
    /// Last played date
    /// </summary>
    public DateTime? PlayDate { get; set; }

    /// <summary>
    /// The number of times as this track has been skipped
    /// </summary>
    public int? SkipCount { get; set; }

    #endregion

    #region Rating

    /// <summary>
    /// Track Rating out of 100
    /// </summary>
    public int? Rating100 { get; set; }

    /// <summary>
    /// Track Rating in Stars
    /// </summary>
    public int? Rating => Rating100.HasValue ? (Rating100 * 5) / 100 : null;

    /// <summary>
    /// Has this track been marked as "loved"
    /// </summary>
    public bool Loved { get; set; }

    #endregion

    #endregion

    public Track()
    {
    }

    public Track(IReadOnlyDictionary<string, dynamic?> properties)
    {
        // parse all important field
        Id = (int)properties["Track ID"]!;
        Size = (int?)properties.GetValueOrDefault("Size", null);
        TotalTime = properties.TryGetValue("Total Time", out var property) ?
            TimeSpan.FromMilliseconds(property) : null;
        Year = (int?)properties.GetValueOrDefault("Year", null);
        Bpm = (int?)properties.GetValueOrDefault("BPM", null);
        DiscNumber = (int?)properties.GetValueOrDefault("Disc Number", null);
        DiscCount = (int?)properties.GetValueOrDefault("Disc Count", null);
        TrackNumber = (int?)properties.GetValueOrDefault("Track Number", null);
        TrackCount = (int?)properties.GetValueOrDefault("Track Count", null);
        DateModified = properties["Date Modified"];
        DateAdded = properties["Date Added"];
        BitRate = (int?)properties.GetValueOrDefault("Bit Rate", null);
        SampleRate = (int?)properties.GetValueOrDefault("Sample Rate", null);
        PlayCount = (int?)properties.GetValueOrDefault("Play Count", null);
        PlayDate = properties.GetValueOrDefault("Play Date UTC", null);
        SkipCount = (int?)properties.GetValueOrDefault("Skip Count", null);
        Rating100 = (int?)properties.GetValueOrDefault("Rating", null);
        Loved = properties.GetValueOrDefault("Loved", false);
        Name = properties.GetValueOrDefault("Name", null);
        Artist = properties.GetValueOrDefault("Artist", null);
        Composer = properties.GetValueOrDefault("Composer", null);
        Album = properties.GetValueOrDefault("Album", null);
        Genre = properties.GetValueOrDefault("Genre", null);
        Location = properties["Location"]!;

        LoadTagInfo();
    }

    public void LoadTagInfo()
    {
        if (!File.Exists(WellFormattedLocation))
        {
            return;
        }
        
        using (TagFile file = TagFile.Create(WellFormattedLocation))
        {
            Type = file.MimeType.Replace("taglib/", "");
            Codec = file.Properties.Description;
            Channels = file.Properties.AudioChannels;
        }
    }

    public void LoadImage()
    {
        using var file = TagFile.Create(WellFormattedLocation);
        
        // if there are any pictures
        if (file.Tag.Pictures.Length >= 1)
        {
            CoverArt = file.Tag.Pictures[0].Data.Data;
        }
    }
}
