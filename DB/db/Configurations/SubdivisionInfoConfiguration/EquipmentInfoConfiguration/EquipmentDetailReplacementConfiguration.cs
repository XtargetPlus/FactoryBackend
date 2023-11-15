using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.SubdivisionInfoConfiguration.EquipmentInfoConfiguration;

public class EquipmentDetailReplacementConfiguration : IEntityTypeConfiguration<EquipmentDetailReplacement>
{
    public void Configure(EntityTypeBuilder<EquipmentDetailReplacement> builder)
    {
        builder.HasKey(edr => new { edr.EquipmentDetailId, edr.EquipmentStatusValueId });

        builder
            .HasOne(edr => edr.EquipmentDetail)
            .WithMany(ed => ed.EquipmentDetailReplacements)
            .HasForeignKey(edr => edr.EquipmentDetailId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(edr => edr.EquipmentStatusValue)
            .WithMany(esv => esv.EquipmentDetailReplacements)
            .HasForeignKey(edr => edr.EquipmentStatusValueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}