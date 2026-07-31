using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Interlink.AspNetCore.Filters;

/// <summary>
/// An exception filter that converts common Interlink exceptions into
/// appropriate HTTP responses (ProblemDetails).
/// </summary>
/// <remarks>
/// Handles <see cref="HandlerNotFoundException"/>. When FluentValidation is referenced
/// by the application, <c>FluentValidation.ValidationException</c> is also mapped to
/// a 400 ValidationProblemDetails response via runtime type checking (no hard dependency).
/// </remarks>
public sealed class InterlinkExceptionFilter : IExceptionFilter
{
    private static readonly Type? ValidationExceptionType =
        Type.GetType("FluentValidation.ValidationException, FluentValidation");

    /// <inheritdoc />
    public void OnException(ExceptionContext context)
    {
        if (context.ExceptionHandled)
            return;

        if (context.Exception is HandlerNotFoundException hnf)
        {
            context.Result = new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Handler not found",
                Detail = hnf.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            })
            {
                StatusCode = StatusCodes.Status404NotFound
            };
            context.ExceptionHandled = true;
            return;
        }

        // Optional FluentValidation support without a package reference
        if (ValidationExceptionType is not null &&
            ValidationExceptionType.IsInstanceOfType(context.Exception))
        {
            var errorsProperty = ValidationExceptionType.GetProperty("Errors");
            if (errorsProperty?.GetValue(context.Exception) is System.Collections.IEnumerable errors)
            {
                var dict = new Dictionary<string, string[]>();
                foreach (var error in errors)
                {
                    var propName = error.GetType().GetProperty("PropertyName")?.GetValue(error) as string ?? string.Empty;
                    var msg = error.GetType().GetProperty("ErrorMessage")?.GetValue(error) as string ?? string.Empty;

                    if (!dict.TryGetValue(propName, out var list))
                    {
                        list = Array.Empty<string>();
                    }
                    dict[propName] = list.Append(msg).ToArray();
                }

                context.Result = new ObjectResult(new ValidationProblemDetails(dict)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "One or more validation errors occurred.",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
