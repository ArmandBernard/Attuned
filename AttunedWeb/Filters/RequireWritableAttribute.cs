using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AttunedWebApi.Filters;

/// <summary>
/// Rejects the request unless the client has explicitly said that read-only mode is off, by sending the
/// <see cref="HEADER_NAME"/> header with the value "false". Anything else, including no header, is treated as
/// read-only. This guards against accidental writes; it is not authorization.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequireWritableAttribute : ActionFilterAttribute
{
    public const string HEADER_NAME = "X-Readonly-Mode";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var value = context.HttpContext.Request.Headers[HEADER_NAME].ToString();

        if (!string.Equals(value, "false", StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Read-only mode is on",
                Detail = $"Send the \"{HEADER_NAME}: false\" header to allow changes to the library."
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
}
