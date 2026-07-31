using FluentValidation;

namespace Interlink.Extensions.Validation;

/// <summary>
/// A pipeline behavior that runs all registered <see cref="IValidator{T}"/> instances
/// for the current request and throws a <see cref="ValidationException"/> when validation fails.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="validators">The collection of validators for <typeparamref name="TRequest"/>.</param>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators ?? Array.Empty<IValidator<TRequest>>();
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)))
                .ConfigureAwait(false);

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
                .ToList();

            if (failures.Count > 0)
                throw new ValidationException(failures);
        }

        return await next(cancellationToken).ConfigureAwait(false);
    }
}