using Interlink.Test.Features;
using Microsoft.AspNetCore.Mvc;

namespace Interlink.Test.Controllers;

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

    [HttpPost]
    public async Task<IActionResult> CreatePet([FromBody] CreatePet.Command command, CancellationToken cancellationToken)
    {
        var pet = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAllPets), new { id = pet.Id }, pet);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePet([FromBody] UpdatePet.Command command, CancellationToken cancellationToken)
    {
        var pet = await sender.Send(command, cancellationToken);
        return Ok(pet);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePet(int id, CancellationToken cancellationToken)
    {
        var pet = await sender.Send(new DeletePet.Command(id), cancellationToken);
        return NoContent();
    }
}
