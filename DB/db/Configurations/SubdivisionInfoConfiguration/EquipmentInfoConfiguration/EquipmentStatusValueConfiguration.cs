using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.SubdivisionInfoConfiguration.EquipmentInfoConfiguration;

public class EquipmentStatusValueConfiguration : IEntityTypeConfiguration<EquipmentStatusValue>
{
    public void Configure(EntityTypeBuilder<EquipmentStatusValue> builder)
    {
        builder.Property(esv => esv.Id).IsRequired();
        builder.Property(esv => esv.Note).HasMaxLength(500);

        builder.HasIndex(esv => esv.Id).IsUnique();

        builder
            .HasOne(esv => esv.Equipment)
            .WithMany(e => e.EquipmentStatusValues)
            .HasForeignKey(esv => esv.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(esv => esv.User)
            .WithMany(u => u.EquipmentStatusValues)
            .HasForeignKey(esv => esv.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(esv => esv.EquipmentFailure)
            .WithMany(ef => ef.EquipmentStatusValues)
            .HasForeignKey(esv => esv.EquipmentFaulureId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(esv => esv.WorkingPart)
            .WithMany(wp => wp.EquipmentStatusValues)
            .HasForeignKey(esv => esv.WorkingPartId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(esv => esv.EquipmentStatus)
            .WithMany(es => es.EquipmentStatusValues)
            .HasForeignKey(esv => esv.EquipmentStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}