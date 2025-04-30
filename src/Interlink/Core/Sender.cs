using Interlink.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Interlink;

internal class Sender(IServiceProvider provider) : ISender
{
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        // Get the request handler for the given request
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = provider.GetService(handlerType)
            ?? throw new InvalidOperationException($"Handler for '{handlerType.FullName}' not found.");

        // Get all pre-processors for the given request type
        var preProcessors = provider.GetServices(typeof(IRequestPreProcessor<>).MakeGenericType(request.GetType()))
            .Cast<dynamic>();

        // Execute all pre-processors before handling the request
        foreach (var processor in preProcessors)
        {
            await processor.Process((dynamic)request, cancellationToken);
        }

        // Create the handler delegate
        RequestHandlerDelegate<TResponse> handlerDelegate = (CancellationToken) => handler.Handle((dynamic)request, cancellationToken);

        // Get all pipeline behaviors
        var behaviors = provider.GetServices(typeof(IPipelineBehavior<,>)
            .MakeGenericType(request.GetType(), typeof(TResponse)))
            .Cast<dynamic>()
            .Reverse() // Chain behaviors in reverse order
            .ToList();

        // Chain the pipeline behaviors around the handler
        foreach (var behavior in behaviors)
        {
            var next = handlerDelegate;
            handlerDelegate = (CancellationToken) => behavior.Handle((dynamic)request, next, cancellationToken);
        }

        // Execute the handler and return the response
        TResponse response = await handlerDelegate();

        // Get all post-processors for the given request type
        var postProcessors = provider.GetServices(typeof(IRequestPostProcessor<,>)
            .MakeGenericType(request.GetType(), typeof(TResponse)))
            .Cast<dynamic>();

        // Execute all post-processors after handling the request
        foreach (var processor in postProcessors)
        {
            await processor.Process((dynamic)request, response, cancellationToken);
        }

        return response;
    }
}

