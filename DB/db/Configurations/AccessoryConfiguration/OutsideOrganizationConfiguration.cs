using DB.Model.AccessoryInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.AccessoryConfiguration;

public class OutsideOrganizationConfiguration : IEntityTypeConfiguration<OutsideOrganization>
{
    public void Configure(EntityTypeBuilder<OutsideOrganization> builder)
    {
        builder.Property(oo => oo.Title).HasMaxLength(255);

        builder.HasIndex(oo => oo.Title).IsUnique();
        builder.HasIndex(oo => oo.Id).IsUnique();
    }
}

