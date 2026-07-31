using Microsoft.Extensions.DependencyInjection;

namespace Interlink.AspNetCore.Filters;

/// <summary>
/// Extension methods for registering Interlink ASP.NET Core integration components.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the <see cref="InterlinkExceptionFilter"/> as a global MVC filter.
    /// Call this after <c>AddControllers()</c> / <c>AddMvc()</c>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that calls can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="services"/> is null.</exception>
    public static IServiceCollection AddInterlinkAspNetCore(this IServiceCollection services)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
        {
            options.Filters.Add<InterlinkExceptionFilter>();
        });

        return services;
    }
}