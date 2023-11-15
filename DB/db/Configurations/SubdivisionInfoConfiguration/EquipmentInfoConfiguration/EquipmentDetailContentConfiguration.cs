using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.SubdivisionInfoConfiguration.EquipmentInfoConfiguration;

public class EquipmentDetailContentConfiguration : IEntityTypeConfiguration<EquipmentDetailContent>
{
    public void Configure(EntityTypeBuilder<EquipmentDetailContent> builder)
    {
        builder.HasKey(edc => new { edc.EquipmentId, edc.EquipmentDetailId });

        builder
            .HasOne(edc => edc.Equipment)
            .WithMany(e => e.EquipmentDetailContents)
            .HasForeignKey(edc => edc.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(edc => edc.EquipmentDetail)
            .WithMany(ed => ed.EquipmentDetailContents)
            .HasForeignKey(edc => edc.EquipmentDetailId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}