namespace Interlink;

public class LoggingPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[Pre] Processing {typeof(TRequest).Name}");
        return Task.CompletedTask;
    }
}