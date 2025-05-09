﻿using Interlink.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Interlink;

internal class Publisher(IServiceProvider provider) : IPublisher
{
    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());
        var handlers = provider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var task = (Task)handlerType
                .GetMethod(nameof(INotificationHandler<TNotification>.Handle))!
                .Invoke(handler, new object[] { notification, cancellationToken })!;

            await task;
        }
    }
}