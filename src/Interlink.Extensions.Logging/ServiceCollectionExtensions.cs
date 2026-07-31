using Microsoft.Extensions.DependencyInjection;

namespace Interlink.Extensions.Logging;

/// <summary>
/// Extension methods for registering Interlink logging behaviors.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the built-in <see cref="LoggingBehavior{TRequest,TResponse}"/>
    /// as an open-generic pipeline behavior.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that calls can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="services"/> is null.</exception>
    public static IServiceCollection AddInterlinkLogging(this IServiceCollection services)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        return services;
    }
}