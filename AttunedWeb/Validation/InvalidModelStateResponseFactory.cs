using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AttunedWebApi.Validation;

/// <summary>
/// Middleware to modify validation error responses to be more helpful
/// </summary>
public static partial class InvalidModelStateResponseFactory
{
    public static IActionResult CreateResponse(ActionContext context)
    {
        var modelState = context.ModelState;

        HandleConversionErrors(modelState);

        var problem = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>()
            .CreateValidationProblemDetails(context.HttpContext, modelState);

        return new BadRequestObjectResult(problem) { ContentTypes = { "application/problem+json" } };
    }

    /// <summary>
    /// Replaces the serializer's CLR-flavoured messages for JSON that cannot be converted ("The JSON value could not be
    /// converted to System.Nullable`1[System.Int32]. Path: ...") with ones that name the field and say what was expected.
    /// </summary>
    private static void HandleConversionErrors(ModelStateDictionary modelState)
    {
        var conversionErrors = modelState
            .SelectMany(entry => entry.Value!.Errors.Select(error => (entry.Key, error.ErrorMessage)))
            .Select(item => (item.Key, Match: ConversionErrorRegex().Match(item.ErrorMessage)))
            .Where(item => item.Match.Success)
            .ToList();

        foreach (var (key, match) in conversionErrors)
        {
            // replace the existing error with our own message, under the plain property name
            modelState.Remove(key);
            var field = match.Groups["path"].Value.Split('.').Last();
            modelState.AddModelError(field, Describe(field, match.Groups["type"].Value));
        }
        
        if (conversionErrors.Count > 0)
        {
            // the body failed to bind, so "the body is required" is a side effect and not useful
            foreach (var key in modelState.Keys.Where(key => !key.StartsWith('$') && modelState[key]!.Errors.Any(
                         error => error.ErrorMessage.EndsWith("field is required."))).ToList())
            {
                modelState.Remove(key);
            }
        }
    }

    private static string Describe(string field, string type)
    {
        var expected = type switch
        {
            _ when type.Contains("Int") => "a whole number",
            _ when type.Contains("Decimal") || type.Contains("Double") || type.Contains("Single") => "a number",
            _ when type.Contains("Boolean") => "true or false",
            _ => null,
        };

        return expected is null ? $"{field} is not valid." : $"{field} must be {expected}.";
    }

    [GeneratedRegex(@"could not be converted to (?<type>\S+?)\. Path: (?<path>\S+)")]
    private static partial Regex ConversionErrorRegex();
}
