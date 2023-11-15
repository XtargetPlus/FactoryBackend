using DB.Model.StorageInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.StorageInfoConfiguration;

public class StoragePlaceConfiguration : IEntityTypeConfiguration<StoragePlace>
{
    public void Configure(EntityTypeBuilder<StoragePlace> builder)
    {
        builder.Property(sp => sp.Id).IsRequired();
        builder.Property(sp => sp.Row).IsRequired();
        builder.Property(sp => sp.Shelf).IsRequired();
        builder.Property(sp => sp.Place).IsRequired();

        builder.HasIndex(sp => sp.Id).IsUnique();

        builder
            .HasOne(sp => sp.Storage)
            .WithMany(s => s.StoragePlaces)
            .HasForeignKey(s => s.StorageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}