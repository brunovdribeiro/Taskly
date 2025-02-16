using Infrastructure.Persistences.Posgres.Tasks;
using Infrastructure.Persistences.Posgres.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistences.Posgres;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options
) : DbContext(options)
{
    public DbSet<TaskSnapshot> TaskSnapshots { get; set; }
    public DbSet<UserSnapshot> UserSnapshots { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder
    )
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}