namespace iTunesSmartParser.Data;

public record TrackDetails : Track
{
    [System.Diagnostics.CodeAnalysis.SetsRequiredMembersAttribute]
    public TrackDetails(Track track)
    {
        Id = track.Id;
        Size = track.Size;
        TotalTime = track.TotalTime; 
        Year = track.Year;
        Bpm = track.Bpm;
        DiscNumber = track.DiscNumber;
        DiscCount = track.DiscCount;
        TrackNumber = track.TrackNumber;
        TrackCount = track.TrackCount;
        DateModified = track.DateModified;
        DateAdded = track.DateAdded;
        BitRate = track.BitRate;
        SampleRate = track.SampleRate;
        PlayCount = track.PlayCount;
        PlayDate = track.PlayDate;
        SkipCount = track.SkipCount;
        Rating = track.Rating;
        Loved = track.Loved;
        Name = track.Name; 
        Artist = track.Artist; 
        Composer = track.Composer; 
        Album = track.Album; 
        Genre = track.Genre; 
        Location = track.Location;
    }
    
    public int? Channels { get; set; }

    public string? Type { get; set; }

    public string? Codec { get; set; }

    public byte[]? CoverArt { get; set; }
}