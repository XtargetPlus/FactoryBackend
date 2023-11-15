using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.SubdivisionInfoConfiguration.EquipmentInfoConfiguration;

public class EquipmentDetailConfiguration : IEntityTypeConfiguration<EquipmentDetail>
{
    public void Configure(EntityTypeBuilder<EquipmentDetail> builder)
    {
        builder.Property(ed => ed.SerialNumber).HasMaxLength(100).IsRequired();
        builder.Property(ed => ed.Title).HasMaxLength(255).IsRequired();
        builder.Property(ed => ed.Id).IsRequired();

        builder.HasIndex(ed => ed.Title);
        builder.HasIndex(ed => ed.Id).IsUnique();
        builder.HasIndex(ed => ed.SerialNumber).IsUnique();
    }
}