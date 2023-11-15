using DB.Model.DetailInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DB.db.Configurations.DetailInfoConfiguration;

public class DetailTypeConfiguration : IEntityTypeConfiguration<DetailType>
{
    public void Configure(EntityTypeBuilder<DetailType> builder)
    {
        builder.Property(dt => dt.Title).HasMaxLength(50);

        builder.HasIndex(dt => dt.Id).IsUnique();
        builder.HasIndex(dt => dt.Title).IsUnique();
    }
}

