namespace Interlink;

/// <summary>
/// Defines a post-processor that runs after the request pipeline and the main handler have completed successfully.
/// </summary>
/// <typeparam name="TRequest">The type of the request that was processed.</typeparam>
/// <typeparam name="TResponse">The type of the response produced by the handler.</typeparam>
public interface IRequestPostProcessor<in TRequest, in TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Processes the request and its response after the handler has completed.
    /// </summary>
    /// <param name="request">The original request instance.</param>
    /// <param name="response">The response produced by the handler.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Process(TRequest request, TResponse response, CancellationToken cancellationToken);
}