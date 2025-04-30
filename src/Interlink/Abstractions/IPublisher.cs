using Interlink.Contracts;

namespace Interlink;

/// <summary>
/// Defines a mechanism for publishing notifications to subscribers.
/// </summary>
public interface IPublisher
{
    /// <summary>
    /// Publishes a notification to all relevant subscribers.
    /// </summary>
    /// <typeparam name="TNotification">The type of the notification being published.</typeparam>
    /// <param name="notification">The notification instance to publish.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous publish operation.</returns>
    Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification;
}
