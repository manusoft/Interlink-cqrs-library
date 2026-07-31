namespace Interlink;

/// <summary>
/// Defines a pre-processor that runs before the request pipeline and the main handler.
/// </summary>
/// <typeparam name="TRequest">The type of the request being processed.</typeparam>
public interface IRequestPreProcessor<in TRequest>
    where TRequest : notnull
{
    /// <summary>
    /// Processes the request before it enters the pipeline.
    /// </summary>
    /// <param name="request">The request instance.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Process(TRequest request, CancellationToken cancellationToken);
}