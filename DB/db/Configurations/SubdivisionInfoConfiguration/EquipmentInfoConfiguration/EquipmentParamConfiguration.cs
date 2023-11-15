using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DB.db.Configurations.SubdivisionInfoConfiguration.EquipmentInfoConfiguration;

public class EquipmentParamConfiguration : IEntityTypeConfiguration<EquipmentParam>
{
    public void Configure(EntityTypeBuilder<EquipmentParam> builder)
    {
        builder.Property(ep => ep.Title).HasMaxLength(200).IsRequired();
        builder.Property(ep => ep.Id).IsRequired();

        builder.HasIndex(ep => ep.Id).IsUnique();
        builder.HasIndex(ep => ep.Title).IsUnique();
    }
}