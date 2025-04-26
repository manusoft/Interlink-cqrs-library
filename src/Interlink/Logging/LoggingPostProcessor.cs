namespace Interlink;

public class LoggingPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
{
    public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[Post] Processed {typeof(TRequest).Name} with response {typeof(TResponse).Name}");
        return Task.CompletedTask;
    }
}