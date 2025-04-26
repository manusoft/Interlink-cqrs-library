using Interlink.Test.Data;
using Interlink.Test.Entities;
using Interlink.Test.Exceptions;

namespace Interlink.Test.Features;

public class DeletePet
{
    public record Command(int Id) : IRequest<Pet>;
    public class Handler(AppDbContext context) : IRequestHandler<Command, Pet>
    {
        public async Task<Pet> Handle(Command request, CancellationToken cancellationToken)
        {
            var pet = await context.Pets.FindAsync(request.Id);
            if (pet == null)
            {
                throw new NotFoundException(nameof(Pet), request.Id);
            }
            context.Pets.Remove(pet);
            await context.SaveChangesAsync(cancellationToken);
            return pet;
        }
    }
}
