using System.Collections.Concurrent;
using System.Reflection;

namespace Interlink;

internal static class TypeScanner
{
    private static readonly ConcurrentDictionary<(Assembly Assembly, Type OpenGenericType), List<(Type, Type)>> _scanCache = new();

    public static IEnumerable<(Type ServiceType, Type ImplementationType)> Scan(Assembly assembly, Type openGenericType)
    {
        return _scanCache.GetOrAdd((assembly, openGenericType), key =>
        {
            return key.Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t =>
                    t.GetInterfaces()
                        .Where(i =>
                            i.IsGenericType &&
                            i.GetGenericTypeDefinition() == key.OpenGenericType
                        )
                        .Select(i => (ServiceType: i, ImplementationType: t))
                )
                .ToList();
        });
    }

    public static IEnumerable<(Type ServiceType, Type ImplementationType)> ScanOpenGenericImplementations(Type openGenericType)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
            .SelectMany(a => SafeGetTypes(a))
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t =>
                t.GetInterfaces()
                    .Where(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == openGenericType
                    )
                    .Select(i => (ServiceType: i, ImplementationType: t))
            );
    }

    private static IEnumerable<Type> SafeGetTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch
        {
            return Array.Empty<Type>();
        }
    }
}
