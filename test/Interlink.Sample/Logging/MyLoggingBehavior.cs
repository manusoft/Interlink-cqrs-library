using Interlink.Contracts;

namespace Interlink.Sample.Logging;

/// <summary>
/// A behavior that logs the handling of requests and responses in the pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class MyLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyLoggingBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    public MyLoggingBehavior() { }

    /// <summary>
    /// Handles the request by logging its processing and invoking the next delegate in the pipeline.
    /// </summary>
    /// <param name="request">The request being handled.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <returns>The response from the next delegate in the pipeline.</returns>
    public async Task<TResponse> Handle(
        TRequest request,        
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"[Behavior1] Handling... {typeof(TRequest).Name}");

        var response = await next();

        Console.WriteLine($"[Behavior1] Handled... {typeof(TRequest).Name}");

        return response;
    }
}

/// <summary>
/// A behavior that logs the handling of requests and responses in the pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class MyLoggingBehavior2<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyLoggingBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    public MyLoggingBehavior2() { }

    /// <summary>
    /// Handles the request by logging its processing and invoking the next delegate in the pipeline.
    /// </summary>
    /// <param name="request">The request being handled.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <returns>The response from the next delegate in the pipeline.</returns>
    public async Task<TResponse> Handle(
        TRequest request,        
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"[Behavior2] Handling... {typeof(TRequest).Name}");

        var response = await next();

        Console.WriteLine($"[Behavior2] Handled... {typeof(TRequest).Name}");

        return response;
    }
}



