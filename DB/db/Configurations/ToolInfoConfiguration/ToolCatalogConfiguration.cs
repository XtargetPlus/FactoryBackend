using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.ToolInfoConfiguration;

public class ToolCatalogConfiguration : IEntityTypeConfiguration<ToolCatalog>
{
    public void Configure(EntityTypeBuilder<ToolCatalog> builder)
    {
        builder.Property(dc => dc.Title).HasMaxLength(255).IsRequired();
        builder.Property(dc => dc.Id).IsRequired();

        builder.HasIndex(dc => dc.Id).IsUnique();
        builder.HasIndex(dc => dc.Title).IsUnique();

        builder
            .HasOne(dc => dc.Father)
            .WithMany(dc => dc.ChildrenToolCatalogs)
            .HasForeignKey(dc => dc.FatherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
