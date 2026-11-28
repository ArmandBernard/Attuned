using System.ComponentModel.DataAnnotations;
using iTunesSmartParser.Data;

namespace AttunedWebApi.Validation;

/// <summary>
/// Like <see cref="RangeAttribute"/>, but for an <see cref="Option{T}"/> number.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class OptionRangeAttribute(int minimum, int maximum = int.MaxValue) : ValidationAttribute
{
    private readonly RangeAttribute _range = new(minimum, maximum);

    public override string FormatErrorMessage(string name) => _range.FormatErrorMessage(name);

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var type = value?.GetType();

        if (type is not { IsGenericType: true } || type.GetGenericTypeDefinition() != typeof(Option<>))
        {
            throw new InvalidOperationException(
                $"{nameof(OptionRangeAttribute)} can only be used on an {nameof(Option<>)}, not on {type?.Name ?? "null"}.");
        }

        var hasValue = (bool) type.GetProperty(nameof(Option<>.HasValue))!.GetValue(value)!;

        if (!hasValue)
        {
            return ValidationResult.Success;
        }

        var inner = type.GetProperty(nameof(Option<>.Value))!.GetValue(value);

        // RangeAttribute accepts null as valid, which is what is wanted for a value that is provided as null
        return _range.IsValid(inner)
            ? ValidationResult.Success
            : new ValidationResult(FormatErrorMessage(validationContext.DisplayName), [validationContext.MemberName!]);
    }
}
