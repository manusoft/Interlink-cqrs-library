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
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = ResolveHandler(handlerType);

        var preProcessors = (_serviceFactory(typeof(IEnumerable<>).MakeGenericType(typeof(IRequestPreProcessor<>).MakeGenericType(request.GetType()))) as IEnumerable<object>)?
            .Cast<dynamic>() ?? Enumerable.Empty<dynamic>();

        foreach (var processor in preProcessors)
        {
            await processor.Process((dynamic)request, cancellationToken);
        }

        RequestHandlerDelegate<TResponse> handlerDelegate = (CancellationToken) => handler.Handle((dynamic)request, cancellationToken);

        var behaviors = (_serviceFactory(typeof(IEnumerable<>).MakeGenericType(typeof(IPipelineBehavior<,>).MakeGenericType(request.GetType(), typeof(TResponse)))) as IEnumerable<object>)?
            .Cast<dynamic>() ?? Enumerable.Empty<dynamic>();

        foreach (var behavior in behaviors.Reverse())
        {
            var next = handlerDelegate;
            handlerDelegate = (CancellationToken) => behavior.Handle((dynamic)request, next, cancellationToken);
        }

        TResponse response = await handlerDelegate();

        var postProcessors = (_serviceFactory(typeof(IEnumerable<>).MakeGenericType(typeof(IRequestPostProcessor<,>).MakeGenericType(request.GetType(), typeof(TResponse)))) as IEnumerable<object>)?
            .Cast<dynamic>() ?? Enumerable.Empty<dynamic>();

        foreach (var processor in postProcessors)
        {
            await processor.Process((dynamic)request, response, cancellationToken);
        }

        return response;
    }
}

