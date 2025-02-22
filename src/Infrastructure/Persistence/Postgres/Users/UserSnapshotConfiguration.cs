using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Postgres.Users;

public class UserSnapshotConfiguration : IEntityTypeConfiguration<UserSnapshot>
{
    public void Configure(
        EntityTypeBuilder<UserSnapshot> builder
    )
    {
        builder.ToTable("Users");

        builder.HasKey(u => new { u.Id, u.Version });
    }
}