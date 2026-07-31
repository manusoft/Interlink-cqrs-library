namespace Interlink.Contracts;

/// <summary>
/// Marker interface for a request that produces a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
public interface IRequest<out TResponse>
{
}

/// <summary>
/// Marker interface for a command-style request that does not return a value.
/// Equivalent to <see cref="IRequest{TResponse}"/> with <see cref="Unit"/> as the response type.
/// </summary>
public interface IRequest : IRequest<Unit>
{
}