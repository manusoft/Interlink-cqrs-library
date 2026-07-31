namespace Interlink;

/// <summary>
/// Represents a delegate that handles a request and produces a response asynchronously.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
/// <returns>A task that represents the asynchronous operation, containing the response.</returns>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>(CancellationToken cancellationToken = default);

/// <summary>
/// Defines a behavior in the request pipeline that can inspect, modify, or short-circuit
/// the handling of a request before and/or after the next component in the pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IPipelineBehavior<in TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Handles the request and optionally calls the next delegate in the pipeline.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="next">The next delegate in the pipeline. Calling this continues processing.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the response.</returns>
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}