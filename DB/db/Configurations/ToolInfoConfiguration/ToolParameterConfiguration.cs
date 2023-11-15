using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.ToolInfoConfiguration;

internal class ToolParameterConfiguration : IEntityTypeConfiguration<ToolParameter>
{
    public void Configure(EntityTypeBuilder<ToolParameter> builder)
    {
        builder.Property(dc => dc.Title).HasMaxLength(255);

        builder.HasIndex(dc => dc.Id).IsUnique();
        builder.HasIndex(dc => dc.Title).IsUnique();

        builder
            .HasOne(tp => tp.Unit)
            .WithMany(u => u.ToolParameters)
            .HasForeignKey(tp => tp.UnitId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
