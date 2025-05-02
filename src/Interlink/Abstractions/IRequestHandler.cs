using Interlink.Contracts;

namespace Interlink;

/// <summary>
/// Defines a handler for processing a request of type <typeparamref name="TRequest"/> and producing a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the specified request and returns a response asynchronously.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response.</returns>
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}


/// <summary>
/// Defines a handler for processing a request of type <typeparamref name="TRequest"/>.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
public interface IRequestHandler<in TRequest> where TRequest : IRequest
{
    /// <summary>
    /// Handles the specified request asynchronously.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Handle(TRequest request, CancellationToken cancellationToken);
}