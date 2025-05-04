using Interlink.Sample.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interlink.Sample.Data;

public class SeedData
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Pets.AnyAsync())
            return;
        var pets = new List<Pet>
        {
            new() { Name = "Fido", Species = "Dog" },
            new() { Name = "Whiskers", Species = "Cat" },
            new() { Name = "Polly", Species = "Parrot" }
        };
        context.Pets.AddRange(pets);
        await context.SaveChangesAsync();
    }
}