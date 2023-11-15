using DB.Model.StorageInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.StorageInfoConfiguration;

public class StoragePlaceMoveDetailConfiguration : IEntityTypeConfiguration<StoragePlaceMoveDetail>
{
    public void Configure(EntityTypeBuilder<StoragePlaceMoveDetail> builder)
    {
        builder.Property(spmd => spmd.Count).IsRequired();
        builder.Property(spmd => spmd.Note).HasMaxLength(500);

        builder.HasKey(spmd => new { spmd.MoveDetailId, spmd.StoragePlaceId });

        builder
            .HasOne(spmd => spmd.StoragePlace)
            .WithMany(sp => sp.StoragePlaceMoveDetails)
            .HasForeignKey(spmd => spmd.StoragePlaceId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(spmd => spmd.MoveDetail)
            .WithMany(md => md.StoragePlaceMoveDetails)
            .HasForeignKey(spmd => spmd.MoveDetailId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}