namespace Interlink;

/// <summary>
/// A behavior that logs the handling of requests and responses in the pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    public LoggingBehavior() { }

    /// <summary>
    /// Handles the request by logging its processing and invoking the next delegate in the pipeline.
    /// </summary>
    /// <param name="request">The request being handled.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <returns>The response from the next delegate in the pipeline.</returns>
    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        Console.WriteLine($"[Behavior] Handling {typeof(TRequest).Name}");

        var response = await next();

        Console.WriteLine($"[Behavior] Handled {typeof(TRequest).Name}");

        return response;
    }
}
