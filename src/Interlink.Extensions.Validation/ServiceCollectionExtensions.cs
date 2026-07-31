using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Interlink.Extensions.Validation;

/// <summary>
/// Extension methods for registering Interlink validation behaviors and FluentValidation validators.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the built-in <see cref="ValidationBehavior{TRequest,TResponse}"/>
    /// as an open-generic pipeline behavior.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that calls can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="services"/> is null.</exception>
    public static IServiceCollection AddInterlinkValidation(this IServiceCollection services)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }

    /// <summary>
    /// Registers the validation behavior and scans the supplied assemblies for
    /// concrete <see cref="IValidator{T}"/> implementations.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">
    /// Assemblies that contain FluentValidation validators.
    /// When omitted, the calling assembly is scanned.
    /// </param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that calls can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="services"/> is null.</exception>
    public static IServiceCollection AddInterlinkValidation(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        services.AddInterlinkValidation();

        if (assemblies is null || assemblies.Length == 0)
            assemblies = new[] { Assembly.GetCallingAssembly() };

        foreach (var assembly in assemblies)
        {
            var validatorTypes = assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>))
                    .Select(i => (Service: i, Implementation: t)));

            foreach (var (service, implementation) in validatorTypes)
            {
                services.AddTransient(service, implementation);
            }
        }

        return services;
    }
}