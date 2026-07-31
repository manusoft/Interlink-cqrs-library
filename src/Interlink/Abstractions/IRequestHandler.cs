using Interlink.Contracts;

namespace Interlink;

/// <summary>
/// Defines a handler for a request of type <typeparamref name="TRequest"/> that returns a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request being handled.</typeparam>
/// <typeparam name="TResponse">The type of response returned by the handler.</typeparam>
public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the given request and returns a response.
    /// </summary>
    /// <param name="request">The request instance to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response.</returns>
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}