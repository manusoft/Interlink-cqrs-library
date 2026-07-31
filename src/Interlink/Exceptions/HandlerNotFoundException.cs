namespace Interlink;

/// <summary>
/// Exception thrown when no handler can be resolved for a given request type.
/// </summary>
public class HandlerNotFoundException : InvalidOperationException
{
    /// <summary>
    /// Gets the request type for which a handler was not found.
    /// </summary>
    public Type RequestType { get; }

    /// <summary>
    /// Gets the expected handler type that could not be resolved, if known.
    /// </summary>
    public Type? HandlerType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HandlerNotFoundException"/> class.
    /// </summary>
    /// <param name="requestType">The request type that has no registered handler.</param>
    /// <param name="handlerType">The expected handler interface type, if known.</param>
    public HandlerNotFoundException(Type requestType, Type? handlerType = null)
        : base(BuildMessage(requestType, handlerType))
    {
        RequestType = requestType ?? throw new ArgumentNullException(nameof(requestType));
        HandlerType = handlerType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HandlerNotFoundException"/> class with a custom message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="requestType">The request type that has no registered handler.</param>
    /// <param name="handlerType">The expected handler interface type, if known.</param>
    public HandlerNotFoundException(string message, Type requestType, Type? handlerType = null)
        : base(message)
    {
        RequestType = requestType ?? throw new ArgumentNullException(nameof(requestType));
        HandlerType = handlerType;
    }

    private static string BuildMessage(Type requestType, Type? handlerType)
    {
        if (handlerType is null)
            return $"No handler registered for request type '{requestType.FullName}'.";

        return $"Handler of type '{handlerType.FullName}' for request '{requestType.FullName}' could not be resolved from the service provider.";
    }
}