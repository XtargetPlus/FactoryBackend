using DB.Model.TechnologicalProcessInfo.TechnologicalProcessDataInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DB.db.DetailInfoConfiguration;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.Property(m => m.Title).HasMaxLength(50);

        builder.HasIndex(m => m.Title).IsUnique();
        builder.HasIndex(m => m.Id).IsUnique();
    }
}

