using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Interlink;

/// <summary>
/// Extension methods for registering Interlink services with an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Interlink core services, scans the supplied assemblies for handlers,
    /// pre/post processors, and registers any explicitly configured pipeline behaviors.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configure">
    /// Optional configuration callback used to register open-generic pipeline behaviors
    /// and to supply a custom service factory.
    /// </param>
    /// <param name="assemblies">
    /// Assemblies to scan for <see cref="IRequestHandler{TRequest,TResponse}"/>,
    /// <see cref="INotificationHandler{TNotification}"/>,
    /// <see cref="IRequestPreProcessor{TRequest}"/> and
    /// <see cref="IRequestPostProcessor{TRequest,TResponse}"/> implementations.
    /// When omitted, the calling assembly is scanned.
    /// </param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that calls can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="services"/> is null.</exception>
    public static IServiceCollection AddInterlink(
        this IServiceCollection services,
        Action<InterlinkOptions>? configure = null,
        params Assembly[] assemblies)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        var options = new InterlinkOptions();
        configure?.Invoke(options);

        if (assemblies is null || assemblies.Length == 0)
            assemblies = new[] { Assembly.GetCallingAssembly() };

        // Register explicitly configured open-generic pipeline behaviors
        foreach (var (behaviorType, _) in options.OpenBehaviors)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), behaviorType);
        }

        // Scan assemblies for handlers and processors
        foreach (var assembly in assemblies)
        {
            RegisterClosedGenericImplementations(services, assembly, typeof(IRequestHandler<,>));
            RegisterClosedGenericImplementations(services, assembly, typeof(INotificationHandler<>));
            RegisterClosedGenericImplementations(services, assembly, typeof(IRequestPreProcessor<>));
            RegisterClosedGenericImplementations(services, assembly, typeof(IRequestPostProcessor<,>));
        }

        // Register core sender / publisher, honouring any custom factory
        if (options.ServiceFactory is not null)
        {
            var factory = options.ServiceFactory;
            services.AddScoped<ISender>(sp => new Sender(sp, factory));
            services.AddScoped<IPublisher>(sp => new Publisher(sp, factory));
        }
        else
        {
            services.AddScoped<ISender, Sender>();
            services.AddScoped<IPublisher, Publisher>();
        }

        // Mediator (composes ISender + IPublisher)
        services.AddScoped<IMediator, Mediator>();

        return services;
    }

    private static void RegisterClosedGenericImplementations(
        IServiceCollection services,
        Assembly assembly,
        Type openGenericType)
    {
        foreach (var (serviceType, implementationType) in TypeScanner.Scan(assembly, openGenericType))
        {
            services.AddScoped(serviceType, implementationType);
        }
    }
}