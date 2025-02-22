using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Postgres.Tasks;

public class TaskSnapshotConfiguration : IEntityTypeConfiguration<TaskSnapshot>
{
    public void Configure(
        EntityTypeBuilder<TaskSnapshot> builder
    )
    {
        builder.ToTable("Tasks");

        builder.HasKey(t => new { t.Id, t.Version });

        builder.Property(t => t.Status)
            .HasConversion<string>();

        builder.Property(t => t.Priority)
            .HasConversion<string>();
    }
}