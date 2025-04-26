namespace Interlink.Core;

public class RequestPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IRequestPreProcessor<TRequest>> _preProcessors;
    private readonly IEnumerable<IRequestPostProcessor<TRequest, TResponse>> _postProcessors;

    public RequestPipelineBehavior(
        IEnumerable<IRequestPreProcessor<TRequest>> preProcessors,
        IEnumerable<IRequestPostProcessor<TRequest, TResponse>> postProcessors)
    {
        _preProcessors = preProcessors;
        _postProcessors = postProcessors;
    }

    // This is the method that matches the signature of the IPipelineBehavior interface.
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        // Execute all pre-processors before proceeding to the next handler or behavior
        foreach (var processor in _preProcessors)
        {
            await processor.Process(request, cancellationToken);
        }

        // Call the next delegate in the pipeline (the actual handler)
        var response = await next();

        // Execute all post-processors after the handler finishes
        foreach (var processor in _postProcessors)
        {
            await processor.Process(request, response, cancellationToken);
        }

        return response;
    }
}