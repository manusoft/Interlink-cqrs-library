namespace Interlink;

/// <summary>
/// A post-processor that logs the processing of a request and its response.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class LoggingPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
{
    /// <summary>
    /// Processes the request and response by logging their types.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="response">The response object.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A completed task.</returns>
    public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[Post] Processed {typeof(TRequest).Name} with response {typeof(TResponse).Name}");
        return Task.CompletedTask;
    }
}
