using iTunesSmartParser.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace AttunedWebApi.Validation;

/// <summary>
/// Stops model validation looking inside an <see cref="Option{T}"/>. It would read the Value property, which throws
/// if no value was provided. Attributes on the property that holds the Option, such as
/// <see cref="OptionRangeAttribute"/>, are still run.
/// </summary>
public class OptionValidationMetadataProvider : IValidationMetadataProvider
{
    public void CreateValidationMetadata(ValidationMetadataProviderContext context)
    {
        var type = context.Key.ModelType;

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Option<>))
        {
            context.ValidationMetadata.ValidateChildren = false;
        }
    }
}
