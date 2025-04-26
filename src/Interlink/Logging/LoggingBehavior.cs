namespace Interlink;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public LoggingBehavior() { }

    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        Console.WriteLine($"[Behavior] Handling {typeof(TRequest).Name}");

        var response = await next();

        Console.WriteLine($"[Behavior] Handled {typeof(TRequest).Name}");

        return response;
    }
}