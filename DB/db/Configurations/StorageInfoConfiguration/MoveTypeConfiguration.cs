using DB.Model.StorageInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.StorageInfoConfiguration;

public class MoveTypeConfiguration : IEntityTypeConfiguration<MoveType>
{
    public void Configure(EntityTypeBuilder<MoveType> builder)
    {
        builder.Property(mt => mt.Title).HasMaxLength(20);

        builder.HasIndex(mt => mt.Title).IsUnique();
        builder.HasIndex(mt => mt.Id).IsUnique();
    }
}

