using Microsoft.Extensions.DependencyInjection;

namespace Interlink;

internal class NotificationExecutor(IServiceProvider provider)
{
    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken) where TNotification : INotification
    {
        var handlers = provider.GetServices<INotificationHandler<TNotification>>();

        foreach (var handler in handlers)
        {
            await handler.Handle(notification, cancellationToken);
        }
    }
}

