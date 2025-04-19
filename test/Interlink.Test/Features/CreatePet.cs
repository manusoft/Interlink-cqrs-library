using Interlink.Abstractions;
using Interlink.Test.Data;
using Interlink.Test.Entities;

namespace Interlink.Test.Features;

public class CreatePet
{
    public record Command(string Name, string Species) : IRequest<Pet>;

    public class Handler(AppDbContext context) : IRequestHandler<Command, Pet>
    {
        public async Task<Pet> Handle(Command request, CancellationToken cancellationToken)
        {
            var pet = new Pet { Name = request.Name, Species = request.Species };
            context.Pets.Add(pet);
            await context.SaveChangesAsync(cancellationToken);
            return pet;
        }
    }
}
