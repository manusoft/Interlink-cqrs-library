using Interlink.Test.Entities;
using Interlink.Test.Features;

namespace Interlink.Test.Logging;

public class MyRequestPostProcessor : IRequestPostProcessor<GetAllPets.Query, List<Pet>>
{
    public Task Process(GetAllPets.Query request, List<Pet> response, CancellationToken cancellationToken)
    {
        Console.WriteLine("[PostProcessor] Processing GetAllPets response...");
        return Task.CompletedTask;
    }
}
