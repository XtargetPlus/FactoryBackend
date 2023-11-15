using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.StorageInfoConfiguration;

public class OperationGraphDetailConfiguration : IEntityTypeConfiguration<OperationGraphDetail>
{
    public void Configure(EntityTypeBuilder<OperationGraphDetail> builder)
    {
        builder.Property(ogd => ogd.Note).HasMaxLength(500);
        builder.Property(ogd => ogd.DetailGraphNumberWithRepeats).HasMaxLength(50);
        builder.Property(ogd => ogd.Id).IsRequired();
        builder.Property(ogd => ogd.Usability).IsRequired();
        builder.Property(ogd => ogd.DetailGraphNumberWithoutRepeats).IsRequired();
        builder.Property(ogd => ogd.IsConfirmed).HasDefaultValue(false);
        builder.Property(ogd => ogd.IsVisible).HasDefaultValue(true);

        builder.HasIndex(ogd => ogd.Id).IsUnique();

        builder.HasQueryFilter(ogd => ogd.IsVisible);

        builder
            .HasOne(ogd => ogd.Detail)
            .WithMany(d => d.OperationGraphDetails)
            .HasForeignKey(ogd => ogd.DetailId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(ogd => ogd.TechnologicalProcess)
            .WithMany(tp => tp.OperationGraphDetails)
            .HasForeignKey(ogd => ogd.TechnologicalProcessId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(ogd => ogd.OperationGraph)
            .WithMany(og => og.OperationGraphDetails)
            .HasForeignKey(ogd => ogd.OperationGraphId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}