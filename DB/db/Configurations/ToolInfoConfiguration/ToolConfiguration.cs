using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DB.Model.ToolInfo;

namespace DB.db.Configurations.ToolInfoConfiguration;
public class ToolConfiguration : IEntityTypeConfiguration<Tool>
{
    public void Configure(EntityTypeBuilder<Tool> builder)
    {
        builder.Property(t => t.Title).HasMaxLength(255);
        builder.Property(t => t.SerialNumber).HasMaxLength(500);
        builder.Property(t => t.Note).HasMaxLength(1000);

        builder.HasIndex(t => t.Id).IsUnique();

    }
}
