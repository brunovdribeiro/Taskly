using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistences.Posgres.Entities;

public class UserSnapshotConfiguration : IEntityTypeConfiguration<UserSnapshot>
{
    public void Configure(
        EntityTypeBuilder<UserSnapshot> builder
    )
    {
        builder.ToTable("user_snapshots");

        builder.HasKey(u => new { u.Id, u.Version });
    }
}