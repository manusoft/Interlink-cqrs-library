namespace Interlink.Internals;

internal static class HandlerResolver
{
    public static object? ResolveHandler(IServiceProvider provider, Type handlerType)
    {
        return provider.GetService(handlerType);
    }
}
