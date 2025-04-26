using Interlink.Sample.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interlink.Sample.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Pet> Pets { get; set; } 
}
