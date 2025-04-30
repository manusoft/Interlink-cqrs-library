using Interlink.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Interlink;

internal class PipelineExecutor(IServiceProvider provider)
{
    public async Task<TResponse> Execute<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
    {
        // Pre-processors
        var preProcessors = provider.GetServices<IRequestPreProcessor<TRequest>>().ToList();
        foreach (var preProcessor in preProcessors)
        {
            await preProcessor.Process(request, cancellationToken);
        }

        // Pipeline Behaviors
        var behaviors = provider.GetServices<IPipelineBehavior<TRequest, TResponse>>().ToList();
        var handler = provider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        // Fix: Explicitly define the Handler method as a lambda to match the delegate signature
        RequestHandlerDelegate<TResponse> currentHandler = (CancellationToken) => handler.Handle(request, cancellationToken);

        foreach (var behavior in behaviors.AsEnumerable().Reverse())
        {
            var next = currentHandler;
            currentHandler = (CancellationToken) => behavior.Handle(request, next, cancellationToken);
        }

        var response = await currentHandler();

        // Post-processors
        var postProcessors = provider.GetServices<IRequestPostProcessor<TRequest, TResponse>>().ToList();
        foreach (var postProcessor in postProcessors)
        {
            await postProcessor.Process(request, response, cancellationToken);
        }

        return response;
    }
}

