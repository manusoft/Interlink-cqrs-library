using Interlink.Contracts;

namespace Interlink;

/// <summary>
/// Defines a sender that dispatches requests to their corresponding handlers and returns responses.
/// </summary>
public interface ISender
{
    /// <summary>
    /// Sends a request and returns the response produced by its handler.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response.</returns>
    /// <exception cref="HandlerNotFoundException">Thrown when no handler is registered for the request type.</exception>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}