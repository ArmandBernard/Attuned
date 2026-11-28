using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using AttunedWebApi.CodeGen;
using AttunedWebApi.Validation;
using iTunesSmartParser.Data;

namespace AttunedWebApi.Dtos.Tracks;

/// <summary>
/// A partial update to a track. Properties left out of the request are not changed. Properties sent as null are
/// cleared (except Rating, which is cleared by sending 0).
/// </summary>
[TypescriptDto]
public record TrackPatchDto : IValidatableObject
{
    public Option<string?> Name { get; init; }
    public Option<string?> Artist { get; init; }
    public Option<string?> Composer { get; init; }
    public Option<string?> Album { get; init; }
    public Option<string?> AlbumArtist { get; init; }
    public Option<string?> Genre { get; init; }
    [OptionRange(0)] public Option<int?> Year { get; init; }
    [OptionRange(0)] public Option<int?> Bpm { get; init; }
    [OptionRange(0)] public Option<int?> DiscNumber { get; init; }
    [OptionRange(0)] public Option<int?> DiscCount { get; init; }
    [OptionRange(0)] public Option<int?> TrackNumber { get; init; }
    [OptionRange(0)] public Option<int?> TrackCount { get; init; }

    /// <summary>
    /// Rating out of 5. 0 is unrated.
    /// </summary>
    [OptionRange(0, 5)] public Option<int> Rating { get; init; }
    public Option<bool?> Loved { get; init; }

    /// <summary>
    /// Anything sent that is not a patchable property, so it can be rejected rather than silently ignored
    /// </summary>
    [JsonExtensionData]
    [JsonInclude]
    internal Dictionary<string, JsonElement>? Unrecognised { get; init; }

    public TrackPatch ToPatch() =>
        new()
        {
            Name = Name,
            Artist = Artist,
            Composer = Composer,
            Album = Album,
            AlbumArtist = AlbumArtist,
            Genre = Genre,
            Year = Year,
            Bpm = Bpm,
            DiscNumber = DiscNumber,
            DiscCount = DiscCount,
            TrackNumber = TrackNumber,
            TrackCount = TrackCount,
            Rating = Rating,
            Loved = Loved,
        };

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var name in Unrecognised?.Keys.ToList() ?? [])
        {
            yield return new ValidationResult($"\"{name}\" is not a property that can be patched.", [name]);
        }
    }
}
