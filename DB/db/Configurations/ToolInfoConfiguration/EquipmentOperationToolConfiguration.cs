using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.ToolInfoConfiguration;

public class EquipmentOperationToolConfiguration : IEntityTypeConfiguration<EquipmentOperationTool>
{
    public void Configure(EntityTypeBuilder<EquipmentOperationTool> builder)
    {
        builder.HasKey(et => et.Id);

        builder
            .HasOne(et => et.Tool)
            .WithMany(t => t.EquipmentOperationTools)
            .HasForeignKey(et => et.ToolId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(et => et.EquipmentOperation)
            .WithMany(t => t.EquipmentOperationTools)
            .HasForeignKey(et => et.EquipmentOperationId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(et => et.Father)
            .WithMany(f => f.Children)
            .HasForeignKey(et => et.FatherId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
