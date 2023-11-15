using DB.Model.WorkInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.WorkInfoConfiguration;

public class EquipmentScheduleConfiguration : IEntityTypeConfiguration<EquipmentSchedule>
{
    public void Configure(EntityTypeBuilder<EquipmentSchedule> builder)
    {
        builder.HasKey(es => new { es.EquipmentId, es.WorkingPartId });

        builder
            .HasOne(es => es.Equipment)
            .WithMany(e => e.EquipmentSchedules)
            .HasForeignKey(e => e.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(es => es.WorkingPart)
            .WithMany(wp => wp.EquipmentSchedules)
            .HasForeignKey(es => es.WorkingPartId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
