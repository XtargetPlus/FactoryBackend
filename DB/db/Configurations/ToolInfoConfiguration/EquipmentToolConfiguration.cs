using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.ToolInfoConfiguration;

internal class EquipmentToolConfiguration : IEntityTypeConfiguration<EquipmentTool>
{
    public void Configure(EntityTypeBuilder<EquipmentTool> builder)
    {
        builder.HasKey(et => new { et.ToolId, et.EquipmentId });

        builder
            .HasOne(et => et.Tool)
            .WithMany(t => t.EquipmentTools)
            .HasForeignKey(et => et.ToolId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(et => et.Equipment)
            .WithMany(e => e.Tools)
            .HasForeignKey(et => et.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}