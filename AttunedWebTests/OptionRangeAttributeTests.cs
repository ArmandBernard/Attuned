using System.ComponentModel.DataAnnotations;
using AttunedWebApi.Validation;
using iTunesSmartParser.Data;

namespace AttunedWebTests;

[TestFixture]
public class OptionRangeAttributeTests
{
    private class Model
    {
        [OptionRange(0, 5)] public Option<int> Rating { get; init; }

        [OptionRange(0)] public Option<int?> Year { get; init; }
    }

    private static List<ValidationResult> Validate(object model)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
        return results;
    }

    [Test]
    public void ValueThatWasNotProvided_IsValid()
    {
        // not provided means leave alone, so there is nothing to check
        Validate(new Model()).Should().BeEmpty();
    }

    [TestCase(0)]
    [TestCase(3)]
    [TestCase(5)]
    public void ValueWithinTheRange_IsValid(int rating)
    {
        Validate(new Model { Rating = rating }).Should().BeEmpty();
    }

    [TestCase(-1)]
    [TestCase(6)]
    [TestCase(int.MinValue)]
    [TestCase(int.MaxValue)]
    public void ValueOutsideTheRange_IsInvalid(int rating)
    {
        var results = Validate(new Model { Rating = rating });

        results.Should().ContainSingle().Which.MemberNames.Should().BeEquivalentTo(nameof(Model.Rating));
        results.Single().ErrorMessage.Should().Contain("Rating").And.Contain("0").And.Contain("5");
    }

    [Test]
    public void ValueProvidedAsNull_IsValid()
    {
        // null means "clear it", which is not a number to range check
        Validate(new Model { Year = Option<int?>.Of(null) }).Should().BeEmpty();
    }

    [Test]
    public void OpenEndedRange_HasNoUpperLimit()
    {
        Validate(new Model { Year = 0 }).Should().BeEmpty();
        Validate(new Model { Year = int.MaxValue }).Should().BeEmpty();
        Validate(new Model { Year = -1 }).Should().ContainSingle();
    }

    [Test]
    public void OnSomethingThatIsNotAnOption_ThrowsSoTheMistakeIsNoticed()
    {
        var attribute = new OptionRangeAttribute(1, 3);

        var act = () => attribute.GetValidationResult(2, new ValidationContext(new Model()) { MemberName = "x" });

        act.Should().Throw<InvalidOperationException>();
    }
}
