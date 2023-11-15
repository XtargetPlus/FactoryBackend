using DB.Model.StatusInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.StatusConfiguration;

public class StatusConfiguration : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        builder.Property(s => s.Title).HasMaxLength(150);
        builder.Property(s => s.TableName).HasMaxLength(50);

        builder.HasIndex(s => s.Title).IsUnique(false);
        builder.HasIndex(s => s.Id).IsUnique();
    }
}

