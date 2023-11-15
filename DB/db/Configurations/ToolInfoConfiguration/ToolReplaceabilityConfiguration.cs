using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.ToolInfoConfiguration;

public class ToolReplaceabilityConfiguration : IEntityTypeConfiguration<ToolReplaceability>
{
    public void Configure(EntityTypeBuilder<ToolReplaceability> builder)
    {
        builder.HasKey(tr => new { tr.MasterId, tr.SlaveId});

        builder
            .HasOne(tr => tr.Master)
            .WithMany(t => t.Slaves)
            .HasForeignKey(tr => tr.MasterId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(tr => tr.Slave)
            .WithMany(t => t.Masters)
            .HasForeignKey(tr => tr.SlaveId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
