namespace Interlink;

/// <summary>
/// A pre-processor that logs the processing of a request of type <typeparamref name="TRequest"/>.
/// </summary>
/// <typeparam name="TRequest">The type of the request being processed.</typeparam>
public class LoggingPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
{
    /// <summary>
    /// Processes the specified request and logs its type.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A completed task.</returns>
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[Pre] Processing {typeof(TRequest).Name}");
        return Task.CompletedTask;
    }
}
