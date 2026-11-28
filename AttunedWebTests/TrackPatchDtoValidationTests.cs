using System.ComponentModel.DataAnnotations;
using AttunedWebApi.Dtos.Tracks;
using iTunesSmartParser.Data;

namespace AttunedWebTests;

public class TrackPatchDtoValidationTests
{
    [Test]
    public void TrackPatchDto_ChecksEachNumberProperty()
    {
        var dtos = new TrackPatchDto[]
        {
            new() {Rating = 6},
            new() {Rating = -1},
            new() {Year = -1},
            new() {Bpm = -1},
            new() {DiscNumber = -1},
            new() {DiscCount = -1},
            new() {TrackNumber = -1},
            new() {TrackCount = -1},
        };

        foreach (var dto in dtos)
        {
            Validate(dto).Should().ContainSingle(because: dto.ToString());
        }
    }

    [Test]
    public void TrackPatchDto_AcceptsValidValuesAndNulls()
    {
        var dto = new TrackPatchDto
        {
            Rating = 5, Year = 2009, Bpm = Option<int?>.Of(null), Name = "x", Loved = Option<bool?>.Of(null)
        };

        Validate(dto).Should().BeEmpty();
    }

    [Test]
    public void TrackPatchDto_RejectsUnrecognisedProperties()
    {
        var dto = System.Text.Json.JsonSerializer.Deserialize<TrackPatchDto>(
            """{"Name":"x","Location":"elsewhere"}""",
            new System.Text.Json.JsonSerializerOptions
            {
                Converters = {new AttunedWebApi.Converters.OptionJsonConverterFactory()}
            })!;

        Validate(dto).Should().ContainSingle().Which.MemberNames.Should().BeEquivalentTo("Location");
    }
    
    private static List<ValidationResult> Validate(object model)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
        return results;
    }
}