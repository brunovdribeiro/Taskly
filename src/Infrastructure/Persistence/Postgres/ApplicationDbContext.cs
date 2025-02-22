using Infrastructure.Persistence.Postgres.Tasks;
using Infrastructure.Persistence.Postgres.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Postgres;

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