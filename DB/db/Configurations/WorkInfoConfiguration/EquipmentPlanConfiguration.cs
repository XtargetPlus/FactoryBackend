using DB.Model.WorkInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.WorkInfoConfiguration;

public class EquipmentPlanConfiguration : IEntityTypeConfiguration<EquipmentPlan>
{
    public void Configure(EntityTypeBuilder<EquipmentPlan> builder)
    {
        builder.HasKey(ep => new { ep.EquipmentId, ep.TechnologicalProcessItemId });

        builder
            .HasOne(ep => ep.Equipment)
            .WithMany(e => e.EquipmentPlans)
            .HasForeignKey(ep => ep.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(ep => ep.TechnologicalProcessItem)
            .WithMany(tpi => tpi.EquipmentPlans)
            .HasForeignKey(ep => ep.TechnologicalProcessItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
