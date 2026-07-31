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
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a void/command request that does not produce a response value.
    /// </summary>
    Task Send(IRequest request, CancellationToken cancellationToken = default);
}