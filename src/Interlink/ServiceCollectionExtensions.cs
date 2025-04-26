using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Interlink;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInterlink(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (assemblies == null || assemblies.Length == 0)
            assemblies = new[] { Assembly.GetCallingAssembly() };

        foreach (var assembly in assemblies)
        {
            // Register IRequestHandler<TRequest, TResponse> for handlers
            var requestHandlers = TypeScanner.Scan(assembly, typeof(IRequestHandler<,>));
            foreach (var (serviceType, implementationType) in requestHandlers)
            {
                services.AddScoped(serviceType, implementationType);
            }

            // Register INotificationHandler<TNotification> for notification handlers
            var notificationHandlers = TypeScanner.Scan(assembly, typeof(INotificationHandler<>));
            foreach (var (serviceType, implementationType) in notificationHandlers)
            {
                services.AddScoped(serviceType, implementationType);
            }

            // Register IPipelineBehavior<TRequest, TResponse> for behaviors
            var pipelineBehaviors = TypeScanner.Scan(assembly, typeof(IPipelineBehavior<,>));
            foreach (var (serviceType, implementationType) in pipelineBehaviors)
            {
                services.AddScoped(serviceType, implementationType);
            }

            // Register IRequestPreProcessor<TRequest> for pre-processors
            var preProcessors = TypeScanner.Scan(assembly, typeof(IRequestPreProcessor<>));
            foreach (var (serviceType, implementationType) in preProcessors)
            {
                services.AddScoped(serviceType, implementationType);
            }

            // Register IRequestPostProcessor<TRequest, TResponse> for post-processors
            var postProcessors = TypeScanner.Scan(assembly, typeof(IRequestPostProcessor<,>));
            foreach (var (serviceType, implementationType) in postProcessors)
            {
                services.AddScoped(serviceType, implementationType);
            }
        }

        // Register core services
        services.AddScoped<ISender, Sender>();
        services.AddScoped<IPublisher, Publisher>();

        return services;
    }
}
