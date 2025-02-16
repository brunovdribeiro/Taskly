using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistences.Posgres.Tasks;

public class TaskSnapshotConfiguration : IEntityTypeConfiguration<TaskSnapshot>
{
    public void Configure(
        EntityTypeBuilder<TaskSnapshot> builder
    )
    {
        builder.ToTable("task_snapshots");

        builder.HasKey(t => new { t.Id, t.Version });

        builder.Property(t => t.Status)
            .HasConversion<string>();

        builder.Property(t => t.Priority)
            .HasConversion<string>();
    }
}