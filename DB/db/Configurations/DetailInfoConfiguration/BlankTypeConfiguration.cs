using DB.Model.TechnologicalProcessInfo.TechnologicalProcessDataInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DB.db.DetailInfoConfiguration;

public class BlankTypeConfiguration : IEntityTypeConfiguration<BlankType>
{
    public void Configure(EntityTypeBuilder<BlankType> builder)
    {
        builder.Property(bt => bt.Title).HasMaxLength(255);

        builder.HasIndex(bt => bt.Title).IsUnique();
        builder.HasIndex(bt => bt.Id).IsUnique();
    }
}

