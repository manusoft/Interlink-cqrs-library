using Interlink.Sample.Entities;
using Interlink.Sample.Features;

namespace Interlink.Sample.Logging;

public class MyRequestPostProcessor : IRequestPostProcessor<GetAllPets.Query, List<Pet>>
{
    public Task Process(GetAllPets.Query request, List<Pet> response, CancellationToken cancellationToken)
    {
        Console.WriteLine("[PostProcessor] Processing GetAllPets response...");
        return Task.CompletedTask;
    }
}
