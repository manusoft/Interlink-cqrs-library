using Interlink.Contracts;
using Interlink.Sample.Data;
using Interlink.Sample.Entities;
using Interlink.Sample.Exceptions;

namespace Interlink.Sample.Features;

public class DeletePet
{
    public record Command(int Id) : IRequest;
    public class Handler(AppDbContext context) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var pet = await context.Pets.FindAsync(request.Id);
            if (pet == null)
            {
                throw new NotFoundException(nameof(Pet), request.Id);
            }
            context.Pets.Remove(pet);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
