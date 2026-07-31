namespace Interlink.Contracts;

/// <summary>
/// Marker interface for a request that produces a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
public interface IRequest<out TResponse> { }
