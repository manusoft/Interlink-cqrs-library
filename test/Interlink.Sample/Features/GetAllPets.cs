using Interlink.Sample.Data;
using Interlink.Sample.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interlink.Sample.Features;

public class GetAllPets
{
    public record Query : IRequest<List<Pet>>;

    public class Handler(AppDbContext context) : IRequestHandler<Query, List<Pet>>
    {
        public async Task<List<Pet>> Handle(Query request, CancellationToken cancellationToken)
        {
            var pets = await context.Pets.ToListAsync();
            return pets;
        }
    }
}
