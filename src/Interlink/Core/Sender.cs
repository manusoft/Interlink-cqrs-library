using Interlink.Contracts;

namespace Interlink;

internal class Sender(IServiceProvider provider, Func<Type, object?>? customFactory = null) : ISender
{
    private readonly Func<Type, object?> _serviceFactory = customFactory ?? provider.GetService;

    private object ResolveHandler(Type handlerType)
    {
        // Use the custom factory or default to IServiceProvider
        var handler = _serviceFactory(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException($"Handler for '{handlerType.FullName}' not found.");
        }

        return handler;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var responseType = typeof(TResponse);

        // Resolve main handler
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
        dynamic handler = ResolveHandler(handlerType);

        // Pre-processors
        var preProcessorType = typeof(IRequestPreProcessor<>).MakeGenericType(requestType);
        var preProcessors = (_serviceFactory(typeof(IEnumerable<>).MakeGenericType(preProcessorType)) as IEnumerable<object>)?.Cast<dynamic>() ?? [];

        foreach (var processor in preProcessors)
            await processor.Process((dynamic)request, cancellationToken);

        // Build handler delegate
        RequestHandlerDelegate<TResponse> handlerDelegate = ct => handler.Handle((dynamic)request, ct);

        // Pipeline behaviors (wrapped in reverse order)
        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType);
        var behaviors = (_serviceFactory(typeof(IEnumerable<>).MakeGenericType(behaviorType)) as IEnumerable<object>)?.Cast<dynamic>() ?? [];

        foreach (var behavior in behaviors.Reverse())
        {
            var next = handlerDelegate;
            handlerDelegate = ct => behavior.Handle((dynamic)request, next, ct);
        }

        // Execute final delegate (via pipeline + handler)
        TResponse response = await handlerDelegate(cancellationToken);

        // Post-processors
        var postProcessorType = typeof(IRequestPostProcessor<,>).MakeGenericType(requestType, responseType);
        var postProcessorInstances = (_serviceFactory(typeof(IEnumerable<>).MakeGenericType(postProcessorType)) as IEnumerable<object>)?.Cast<dynamic>() ?? [];

        foreach (var processor in postProcessorInstances)
            await processor.Process((dynamic)request, response, cancellationToken);

        return response;
    }
}

