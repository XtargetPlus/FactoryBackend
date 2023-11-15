using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.StorageInfoConfiguration;

public class OperationGraphDetailItemConfiguration : IEntityTypeConfiguration<OperationGraphDetailItem>
{
    public void Configure(EntityTypeBuilder<OperationGraphDetailItem> builder)
    {
        builder.Property(ogdi => ogdi.Note).HasMaxLength(500);
        builder.Property(ogdi => ogdi.Id).IsRequired();
        builder.Property(ogdi => ogdi.OrdinalNumber).IsRequired();

        builder.HasIndex(ogdi => ogdi.Id).IsUnique();

        builder
            .HasOne(ogdi => ogdi.OperationGraphDetail)
            .WithMany(ogd => ogd.OperationGraphDetailItems)
            .HasForeignKey(ogdi => ogdi.OperationGraphDetailId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(ogdi => ogdi.TechnologicalProcessItem)
            .WithMany(tpi => tpi.OperationGraphDetailItems)
            .HasForeignKey(ogdi => ogdi.TechnologicalProcessItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}