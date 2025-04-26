namespace Interlink;

/// <summary>
/// Defines a pre-processor for a specific type of request.
/// </summary>
/// <typeparam name="TRequest">The type of the request to process.</typeparam>
public interface IRequestPreProcessor<TRequest>
{
    /// <summary>
    /// Processes the specified request before it is handled.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Process(TRequest request, CancellationToken cancellationToken);
}
