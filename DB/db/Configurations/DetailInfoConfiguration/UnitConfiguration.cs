using DB.Model.DetailInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DB.db.Configurations.DetailInfoConfiguration;

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.Property(u => u.Title).HasMaxLength(50);

        builder.HasIndex(u => u.Title).IsUnique();
        builder.HasIndex(u => u.Id).IsUnique();
    }
}

