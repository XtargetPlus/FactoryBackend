using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.StorageInfoConfiguration;

public class OperationGraphGroupConfiguration : IEntityTypeConfiguration<OperationGraphGroup>
{
    public void Configure(EntityTypeBuilder<OperationGraphGroup> builder)
    {
        builder.HasKey(ogg => new { ogg.OperationGraphNextId, ogg.OperationGraphMainId });

        builder
            .HasOne(ogg => ogg.OperationGraphMain)
            .WithMany(ogm => ogm.OperationGraphMainGroups)
            .HasForeignKey(ogg => ogg.OperationGraphMainId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(ogg => ogg.OperationGraphNext)
            .WithMany(ogm => ogm.OperationGraphNextGroups)
            .HasForeignKey(ogg => ogg.OperationGraphNextId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}