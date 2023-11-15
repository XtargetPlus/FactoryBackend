using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.ToolInfoConfiguration;
public class ToolChildConfiguration : IEntityTypeConfiguration<ToolChild>
{
    public void Configure(EntityTypeBuilder<ToolChild> builder)
    {
        builder.HasKey(dc => new { dc.FatherId, dc.ChildId });

        builder.HasIndex(dc => dc.Count);
        builder.HasIndex(dc => dc.Priority);

        builder
            .HasOne(dc => dc.Father)
            .WithMany(c => c.ToolChildren)
            .HasForeignKey(dc => dc.FatherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(dc => dc.Child)
            .WithMany(c => c.ToolFathers)
            .HasForeignKey(dc => dc.ChildId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
