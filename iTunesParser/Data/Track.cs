using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TagFile = TagLib.File;

namespace iTunesSmartParser;

public class Track
{
    #region Properties

    /// <summary>
    /// The unique ID of this track
    /// </summary>
    public int TrackID { get; set; }

    public Dictionary<string, string> AllProperties;

    #region File

    public string MediaKind => "Music";

    public FileInfo FileI { get { return new FileInfo("\\\\?\\" + WellFormattedLocation); } }

    /// <summary>
    /// The size of the track in bytes
    /// </summary>
    public int? Size { get; set; }

    /// <summary>
    /// The size of the track in MB (where 1kB = 1024 bytes etc.)
    /// </summary>
    public float? SizeMB { get { return Size.HasValue ? Size / 1024F / 1024F : null; } }

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

    public int Channels { get; set; }

    public string Type { get; set; }

    public string Codec { get; set; }

    public byte[] CoverArt;

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
    public int? BPM { get; set; }

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
    public int PlayCount { get; set; }

    /// <summary>
    /// Last played date
    /// </summary>
    public DateTime? PlayDate { get; set; }

    /// <summary>
    /// The number of times as this track has been skipped
    /// </summary>
    public int SkipCount { get; set; }

    #endregion

    #region Rating

    /// <summary>
    /// Track Rating out of 100
    /// </summary>
    public int? Rating100;

    /// <summary>
    /// Track Rating in Stars
    /// </summary>
    public int? Rating { get { return Rating100.HasValue ? (Rating100 * 5) / 100 : null; } }

    /// <summary>
    /// Has this track been marked as "loved"
    /// </summary>
    public bool Loved { get; set; }

    #endregion

    #endregion

    public Track(Dictionary<string, string> properties)
    {
        Dictionary<string, string> p = properties;

        AllProperties = p;

        // parse all important field
        TrackID = int.Parse(p["Track ID"]);
        Size = p.ContainsKey("Size") ? (int?)int.Parse(p["Size"]) : null;
        TotalTime = p.ContainsKey("Total Time") ?
            (TimeSpan?)TimeSpan.FromMilliseconds(int.Parse(p["Total Time"])) : null;
        Year = p.ContainsKey("Year") ? (int?)int.Parse(p["Year"]) : null;
        BPM = p.ContainsKey("BPM") ? (int?)int.Parse(p["BPM"]) : null;
        DiscNumber = p.ContainsKey("Disc Number") ? (int?)int.Parse(p["Track Number"]) : null;
        DiscCount = p.ContainsKey("Disc Count") ? (int?)int.Parse(p["Disc Count"]) : null;
        TrackNumber = p.ContainsKey("Track Number") ? (int?)int.Parse(p["Track Number"]) : null;
        TrackCount = p.ContainsKey("Track Count") ? (int?)int.Parse(p["Track Count"]) : null;
        DateModified = DateTime.Parse(p["Date Modified"]);
        DateAdded = DateTime.Parse(p["Date Added"]);
        BitRate = p.ContainsKey("Bit Rate") ? (int?)int.Parse(p["Bit Rate"]) : null;
        SampleRate = p.ContainsKey("Sample Rate") ? (int?)int.Parse(p["Sample Rate"]) : null;
        PlayCount = p.ContainsKey("Play Count") ? int.Parse(p["Play Count"]) : 0;
        PlayDate = p.ContainsKey("Play Date UTC") ?
            (DateTime?)DateTime.Parse(p["Play Date UTC"]) : null;
        SkipCount = p.ContainsKey("Skip Count") ? int.Parse(p["Skip Count"]) : 0;
        Rating100 = p.ContainsKey("Rating") ? (int?)int.Parse(p["Rating"]) : null;
        Loved = p.ContainsKey("Loved");
        Name = p.ContainsKey("Name") ? p["Name"] : null;
        Artist = p.ContainsKey("Artist") ? p["Artist"] : null;
        Composer = p.ContainsKey("Composer") ? p["Composer"] : null;
        Album = p.ContainsKey("Album") ? p["Album"] : null;
        Genre = p.ContainsKey("Genre") ? p["Genre"] : null;
        Location = p["Location"];

        LoadTagInfo();
    }

    public void LoadTagInfo()
    {
        // don't even try if the file doesn't exist
        if (!FileI.Exists) { return; }

        using (TagFile file = TagFile.Create(WellFormattedLocation))
        {
            Type = file.MimeType.Replace("taglib/", "");
            Codec = file.Properties.Description;
            Channels = file.Properties.AudioChannels;
        }
    }

    public void LoadImage()
    {
        // don't even try if the file doesn't exist
        if (!FileI.Exists) { return; }

        using (TagFile file = TagFile.Create(WellFormattedLocation))
        {
            // if there are any pictures
            if (file.Tag.Pictures.Length >= 1)
            {
                CoverArt = file.Tag.Pictures[0].Data.Data;
            }
        }
    }
}
