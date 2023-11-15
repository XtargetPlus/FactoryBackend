using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.ToolInfoConfiguration;

public class ToolCatalogConcreteConfiguration : IEntityTypeConfiguration<ToolCatalogConcrete>
{
    public void Configure(EntityTypeBuilder<ToolCatalogConcrete> builder)
    {
        builder.HasKey(tcc => new { tcc.ToolId, tcc.ToolCatalogId });

        builder
            .HasOne(tcc => tcc.ToolCatalogs)
            .WithMany(tc => tc.ToolCatalogsConcretes)
            .HasForeignKey(tcc => tcc.ToolCatalogId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(tcc => tcc.Tool)
            .WithMany(t => t.ToolCatalogsConcretes)
            .HasForeignKey(tcc => tcc.ToolId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}