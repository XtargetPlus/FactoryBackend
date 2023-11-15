using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.SubdivisionInfoConfiguration.EquipmentInfoConfiguration;

public class EquipmentOperationConfiguration : IEntityTypeConfiguration<EquipmentOperation>
{
    public void Configure(EntityTypeBuilder<EquipmentOperation> builder)
    {
        builder.Property(eo => eo.DebugTime).HasColumnType("float");
        builder.Property(eo => eo.LeadTime).HasColumnType("float");
        builder.Property(eo => eo.Priority).HasColumnType("tinyint unsigned");
        builder.Property(eo => eo.Note).HasMaxLength(1000);
        builder.Property(eo => eo.Id).IsRequired();

        builder.HasIndex(eo => eo.Id).IsUnique();

        builder
            .HasOne(eo => eo.Equipment)
            .WithMany(e => e.EquipmentOperations)
            .HasForeignKey(eo => eo.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(eo => eo.TechnologicalProcessItem)
            .WithMany(tp => tp.EquipmentOperations)
            .HasForeignKey(eo => eo.TechnologicalProcessItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}