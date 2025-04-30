namespace Interlink.Contracts;

/// <summary>
/// Represents a request in the Interlink pipeline.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IRequest<out TResponse> { }

/// <summary>
/// Represents a request in the Interlink pipeline.
/// </summary>
public interface IRequest { }
