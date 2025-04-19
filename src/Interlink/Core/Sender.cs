using Interlink.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Interlink.Core;

internal class Sender(IServiceProvider provider) : ISender
{
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = provider.GetService(handlerType)
        ?? throw new InvalidOperationException($"Handler for '{handlerType.FullName}' not found.");

        var behaviors = provider.GetServices(typeof(IPipelineBehavior<,>)
            .MakeGenericType(request.GetType(), typeof(TResponse))).Cast<dynamic>().Reverse().ToList();

        RequestHandlerDelegate<TResponse> handlerDelegate = () => handler.Handle((dynamic)request, cancellationToken);

        foreach (var behavior in behaviors)
        {
            var next = handlerDelegate;
            handlerDelegate = () => behavior.Handle((dynamic)request, cancellationToken, next);
        }

        return await handlerDelegate();
    }
}
