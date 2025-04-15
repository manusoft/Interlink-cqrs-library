using Microsoft.AspNetCore.Mvc;

namespace Interlink.Test.Features;

public class GetAllPets
{
    public record Query : IRequest<List<string>>;

    public class Handler : IRequestHandler<Query, List<string>>
    {
        public Task<List<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            var pets = new List<string> { "Dog", "Cat", "Fish" };
            return Task.FromResult(pets);
        }
    }
}

[ApiController]
[Route("api/[controller]")]
public class PetController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllPets(CancellationToken cancellationToken)
    {
        var pets = await sender.Send(new GetAllPets.Query(), cancellationToken);
        return Ok(pets);
    }
}
