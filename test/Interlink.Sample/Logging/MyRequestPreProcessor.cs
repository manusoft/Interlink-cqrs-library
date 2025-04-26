using Interlink.Sample.Features;

namespace Interlink.Sample.Logging;

public class MyRequestPreProcessor : IRequestPreProcessor<GetAllPets.Query>
{
    public Task Process(GetAllPets.Query request, CancellationToken cancellationToken)
    {
        Console.WriteLine("[PreProcessor] Processing GetAllPets request...");
        return Task.CompletedTask;
    }
}

