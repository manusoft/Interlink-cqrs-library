namespace Interlink;

public interface IRequestPreProcessor<TRequest>
{
    Task Process(TRequest request, CancellationToken cancellationToken);
}