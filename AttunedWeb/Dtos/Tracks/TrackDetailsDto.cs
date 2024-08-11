using AttunedWebApi.CodeGen;
using iTunesSmartParser.Data;
using Reinforced.Typings.Attributes;

namespace AttunedWebApi.Dtos.Tracks;

[TypescriptDto]
public record TrackDetailsDto
{
    public required int Id { get; init; }
    public required string Location { get; init; }

    [TsProperty(Type= "\"Music\"")]
    public string Media => "Music";
    public int? Size { get; init; }
    public DateTime DateModified { get; init; }
    public DateTime DateAdded { get; init; }
    public int? BitRate { get; init; }
    public int? SampleRate { get; init; }
    public TimeSpan? TotalTime { get; init; }
    public int? Year { get; init; }
    public int? Bpm { get; init; }
    public int? DiscNumber { get; init; }
    public int? DiscCount { get; init; }
    public int? TrackNumber { get; init; }
    public int? TrackCount { get; init; }
    public string? Name { get; init; }
    public string? Artist { get; init; }
    public string? Composer { get; init; }
    public string? Album { get; init; }
    public string? Genre { get; init; }
    public int? PlayCount { get; init; }
    public DateTime? PlayDate { get; init; }
    public int? SkipCount { get; init; }
    public int Rating { get; init; }
    public bool? Loved { get; init; }
    
    
    public int? Channels { get; set; }

    public string? Type { get; set; }

    public string? Codec { get; set; }

    /// <summary>
    /// Base64 encoded image data 
    /// </summary>
    public string? CoverArt { get; set; }

    public static TrackDetailsDto FromTrackDetails(TrackDetails track) =>
        new()
        {
            Id = track.Id,
            Size = track.Size,
            DateModified = track.DateModified,
            DateAdded = track.DateAdded,
            Location = track.Location,
            BitRate = track.BitRate,
            SampleRate = track.SampleRate,
            TotalTime = track.TotalTime,
            Year = track.Year,
            Bpm = track.Bpm,
            DiscNumber = track.DiscNumber,
            DiscCount = track.DiscCount,
            TrackNumber = track.TrackNumber,
            TrackCount = track.TrackCount,
            Name = track.Name,
            Artist = track.Artist,
            Composer = track.Composer,
            Album = track.Album,
            Genre = track.Genre,
            PlayCount = track.PlayCount,
            PlayDate = track.PlayDate,
            SkipCount = track.SkipCount,
            Rating = track.Rating,
            Loved = track.Loved,
            
            Channels = track.Channels,
            Type = track.Type,
            Codec = track.Codec,
            CoverArt = track.CoverArt != null ? Convert.ToBase64String(track.CoverArt) : null
        };
}