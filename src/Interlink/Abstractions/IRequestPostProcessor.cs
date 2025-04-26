namespace Interlink;

public interface IRequestPostProcessor<TRequest, TResponse>
{
    Task Process(TRequest request, TResponse response, CancellationToken cancellationToken);
}