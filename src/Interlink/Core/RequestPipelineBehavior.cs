namespace Interlink.Core;

/// <summary>
/// Represents a pipeline behavior that processes a request before and after it is handled by the main handler.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class RequestPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IRequestPreProcessor<TRequest>> _preProcessors;
    private readonly IEnumerable<IRequestPostProcessor<TRequest, TResponse>> _postProcessors;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestPipelineBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="preProcessors">The collection of pre-processors to execute before the request is handled.</param>
    /// <param name="postProcessors">The collection of post-processors to execute after the request is handled.</param>
    public RequestPipelineBehavior(
        IEnumerable<IRequestPreProcessor<TRequest>> preProcessors,
        IEnumerable<IRequestPostProcessor<TRequest, TResponse>> postProcessors)
    {
        _preProcessors = preProcessors;
        _postProcessors = postProcessors;
    }

    /// <summary>
    /// Handles the request by executing pre-processors, invoking the next handler, and then executing post-processors.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="next">The delegate to invoke the next handler in the pipeline.</param>
    /// <returns>The response from the next handler in the pipeline.</returns>
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
