using Infrastructure.Persistences.Posgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistences.Posgres;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options
    ) : base(options) { }

    public DbSet<TaskSnapshot> TaskSnapshots { get; set; }
    public DbSet<UserSnapshot> UserSnapshots { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder
    )
    {
        modelBuilder.ApplyConfiguration(new TaskSnapshotConfiguration());
        modelBuilder.ApplyConfiguration(new UserSnapshotConfiguration());
    }
}