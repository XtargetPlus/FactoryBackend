using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.SubdivisionInfoConfiguration.EquipmentInfoConfiguration;

public class EquipmentFailureConfiguration : IEntityTypeConfiguration<EquipmentFailure>
{
    public void Configure(EntityTypeBuilder<EquipmentFailure> builder)
    {
        builder.Property(ef => ef.Title).HasMaxLength(150).IsRequired();
        builder.Property(ef => ef.Id).IsRequired();

        builder.HasIndex(ef => ef.Id).IsUnique();
        builder.HasIndex(ef => ef.Title).IsUnique();
    }
}