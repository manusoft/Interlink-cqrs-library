using Interlink.Test.Data;
using Interlink.Test.Entities;
using Interlink.Test.Exceptions;

namespace Interlink.Test.Features;

public class UpdatePet
{
    public record Command(int Id, string Name, string Species) : IRequest<Pet>;
    public class Handler(AppDbContext context) : IRequestHandler<Command, Pet>
    {
        public async Task<Pet> Handle(Command request, CancellationToken cancellationToken)
        {
            var pet = await context.Pets.FindAsync(request.Id);
            if (pet == null)
            {
                throw new NotFoundException(nameof(Pet), request.Id);
            }
            pet.Name = request.Name;
            pet.Species = request.Species;
            await context.SaveChangesAsync(cancellationToken);
            return pet;
        }
    }
}