using Interlink.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Interlink.Processing;

internal class PipelineExecutor(IServiceProvider provider)
{
    public Task<TResponse> Execute<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken) where TRequest : IRequest<TResponse>
    {
        var behaviors = provider.GetServices<IPipelineBehavior<TRequest, TResponse>>().ToList();
        var handler = provider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        // Define a local function for the handler
        Task<TResponse> Handler() => handler.Handle(request, cancellationToken);

        // Use a variable to hold the delegate and update it
        RequestHandlerDelegate<TResponse> currentHandler = Handler;

        foreach (var behavior in behaviors.AsEnumerable().Reverse())
        {
            var next = currentHandler;
            currentHandler = () => behavior.Handle(request, cancellationToken, next);
        }

        return currentHandler();
    }
}

