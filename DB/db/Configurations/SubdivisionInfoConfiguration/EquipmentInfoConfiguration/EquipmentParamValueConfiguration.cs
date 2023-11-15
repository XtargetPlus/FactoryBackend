using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.SubdivisionInfoConfiguration.EquipmentInfoConfiguration;

public class EquipmentParamValueConfiguration : IEntityTypeConfiguration<EquipmentParamValue>
{
    public void Configure(EntityTypeBuilder<EquipmentParamValue> builder)
    {
        builder.Property(epv => epv.Value).HasMaxLength(254).IsRequired();

        builder.HasKey(epv => new { epv.EquipmentId, epv.EquipmentParamId });

        builder
            .HasOne(epv => epv.Equipment)
            .WithMany(e => e.EquipmentParamValues)
            .HasForeignKey(epv => epv.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(epv => epv.EquipmentParam)
            .WithMany(ep => ep.EquipmentParamValues)
            .HasForeignKey(epv => epv.EquipmentParamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}