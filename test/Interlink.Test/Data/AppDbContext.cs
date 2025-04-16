using Interlink.Test.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interlink.Test.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Pet> Pets { get; set; } 
}
