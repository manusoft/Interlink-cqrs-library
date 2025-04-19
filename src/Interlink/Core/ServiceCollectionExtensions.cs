using Interlink.Abstractions;
using Interlink.Internals;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Interlink.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInterlink(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();

        services.AddScoped<ISender, Sender>();
        services.AddScoped<IPublisher, Publisher>();

        // Register IRequestHandler<TRequest, TResponse>
        var requestHandlers = TypeScanner.Scan(assembly, typeof(IRequestHandler<,>));
        foreach (var (serviceType, implementationType) in requestHandlers)
        {
            services.AddScoped(serviceType, implementationType);
        }

        // Register INotificationHandler<TNotification>
        var notificationHandlers = TypeScanner.Scan(assembly, typeof(INotificationHandler<>));
        foreach (var (serviceType, implementationType) in notificationHandlers)
        {
            services.AddScoped(serviceType, implementationType);
        }

        // Register IPipelineBehavior<TRequest, TResponse>
        var pipelineBehaviors = TypeScanner.Scan(assembly, typeof(IPipelineBehavior<,>));
        foreach (var (serviceType, implementationType) in pipelineBehaviors)
        {
            services.AddScoped(serviceType, implementationType);
        }

        return services;
    }
}
