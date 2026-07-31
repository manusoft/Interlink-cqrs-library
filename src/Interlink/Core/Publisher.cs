using Interlink.Contracts;

namespace Interlink;

/// <summary>
/// Default implementation of <see cref="IPublisher"/> that resolves all
/// notification handlers and invokes them sequentially.
/// </summary>
internal sealed class Publisher : IPublisher
{
    private readonly IServiceProvider _provider;
    private readonly Func<Type, object?> _serviceFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="Publisher"/> class.
    /// </summary>
    /// <param name="provider">The service provider used for resolution when no custom factory is supplied.</param>
    /// <param name="customFactory">
    /// Optional custom factory. When provided it is used instead of <paramref name="provider"/>
    /// for resolving notification handlers.
    /// </param>
    public Publisher(IServiceProvider provider, Func<Type, object?>? customFactory = null)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _serviceFactory = customFactory ?? (type => _provider.GetService(type));
    }

    /// <inheritdoc />
    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        if (notification is null)
            throw new ArgumentNullException(nameof(notification));

        var notificationType = notification.GetType();
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notificationType);
        var enumerableType = typeof(IEnumerable<>).MakeGenericType(handlerType);

        var handlers = _serviceFactory(enumerableType) as IEnumerable<object> ?? Array.Empty<object>();

        foreach (var handler in handlers)
        {
            // Use dynamic for consistency with Sender and to avoid reflection Invoke overhead
            await ((dynamic)handler).Handle((dynamic)notification, cancellationToken).ConfigureAwait(false);
        }
    }
}