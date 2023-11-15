using DB.Model.StorageInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.StorageInfoConfiguration;

public class StorageConfiguration : IEntityTypeConfiguration<Storage>
{
    public void Configure(EntityTypeBuilder<Storage> builder)
    {
        builder.Property(s => s.Title).HasMaxLength(255).IsRequired();
        builder.Property(s => s.Id).IsRequired();
        builder.Property(s => s.IsPhysicalStorage).IsRequired();

        builder.HasIndex(s => s.Id).IsUnique();
        builder.HasIndex(s => s.Title).IsUnique();

        builder
            .HasOne(s => s.Subdivision)
            .WithMany(s => s.Storages)
            .HasForeignKey(s => s.SubdivisionId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(s => s.FatherStorage)
            .WithMany(fs => fs.ChildrenStorages)
            .HasForeignKey(s => s.FatherStorageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}