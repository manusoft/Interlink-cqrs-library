using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Interlink;

/// <summary>
/// Provides extension methods for registering Interlink services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Interlink services in the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configure"></param>
    /// <param name="assemblies">The assemblies to scan for handlers and behaviors. If not provided, the calling assembly is used.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddInterlink(this IServiceCollection services, Action<InterlinkOptions>? configure = null, params Assembly[] assemblies)
    {
        var options = new InterlinkOptions();
        configure?.Invoke(options);

        if (assemblies == null || assemblies.Length == 0)
            assemblies = new[] { Assembly.GetCallingAssembly() };


        foreach (var assembly in assemblies)
        {
            // Register explicitly configured pipeline behaviors first
            foreach (var openBehavior in options.OpenBehaviors)
            {
                services.AddScoped(typeof(IPipelineBehavior<,>), openBehavior);
            }

            // Register IRequestHandler<TRequest, TResponse> for handlers
            RegisterGenericTypes(services, assembly, typeof(IRequestHandler<,>));

            // Register INotificationHandler<TNotification> for notification handlers
            RegisterGenericTypes(services, assembly, typeof(INotificationHandler<>));

            // Register IRequestPreProcessor<TRequest> for pre-processors
            RegisterGenericTypes(services, assembly, typeof(IRequestPreProcessor<>));

            // Register IRequestPostProcessor<TRequest, TResponse> for post-processors
            RegisterGenericTypes(services, assembly, typeof(IRequestPostProcessor<,>));
        }

        // Register core services
        services.AddScoped<ISender, Sender>();
        services.AddScoped<IPublisher, Publisher>();        

        return services;
    }

    private static void RegisterGenericTypes(IServiceCollection services, Assembly assembly, Type openGenericType)
    {
        var types = GetTypesFromAssembly(assembly, openGenericType);
        foreach (var (serviceType, implementationType) in types)
        {
            services.AddScoped(serviceType, implementationType);
        }
    }

    private static IEnumerable<(Type ServiceType, Type ImplementationType)> GetTypesFromAssembly(Assembly assembly, Type openGenericType)
    {
        if (!_cachedTypes.ContainsKey(assembly))
        {
            var types = TypeScanner.Scan(assembly, openGenericType).ToList();
            _cachedTypes[assembly] = types;
        }

        return _cachedTypes[assembly];
    }

    private static readonly Dictionary<Assembly, List<(Type ServiceType, Type ImplementationType)>> _cachedTypes = new();
}
