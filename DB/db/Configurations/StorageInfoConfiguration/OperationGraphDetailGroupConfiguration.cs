using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.StorageInfoConfiguration;

public class OperationGraphDetailGroupConfiguration : IEntityTypeConfiguration<OperationGraphDetailGroup>
{
    public void Configure(EntityTypeBuilder<OperationGraphDetailGroup> builder)
    {
        builder.HasKey(ogdg => new { ogdg.OperationGraphNextDetailId, ogdg.OperationGraphMainDetailId });

        builder
            .HasOne(ogdg => ogdg.OperationGraphNextDetail)
            .WithMany(ognd => ognd.OperationGraphNextDetails)
            .HasForeignKey(ogdg => ogdg.OperationGraphNextDetailId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(ogdg => ogdg.OperationGraphMainDetail)
            .WithMany(ognd => ognd.OperationGraphMainDetails)
            .HasForeignKey(ogdg => ogdg.OperationGraphMainDetailId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}