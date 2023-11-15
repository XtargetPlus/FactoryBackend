using DB.Model.AccessoryInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.AccessoryConfiguration;

public class AccessoryTypeConfiguration : IEntityTypeConfiguration<AccessoryType>
{
    public void Configure(EntityTypeBuilder<AccessoryType> builder)
    {
        builder.Property(at => at.Title).HasMaxLength(50);

        builder.HasIndex(at => at.Title).IsUnique();
        builder.HasIndex(at => at.Id).IsUnique();
    }
}

