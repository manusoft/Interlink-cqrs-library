namespace Interlink;

/// <summary>
/// Defines a post-processor for handling a request and its corresponding response.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IRequestPostProcessor<TRequest, TResponse>
{
    /// <summary>
    /// Processes the request and response after the main handling logic has been executed.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="response">The response object.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Process(TRequest request, TResponse response, CancellationToken cancellationToken);
}
