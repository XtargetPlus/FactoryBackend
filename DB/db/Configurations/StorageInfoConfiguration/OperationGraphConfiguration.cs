using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.StorageInfoConfiguration;

public class OperationGraphConfiguration : IEntityTypeConfiguration<OperationGraph>
{
    public void Configure(EntityTypeBuilder<OperationGraph> builder)
    {
        builder.Property(og => og.Note).HasMaxLength(500);
        builder.Property(og => og.Id).IsRequired();
        builder.Property(og => og.Priority).IsRequired();
        builder.Property(og => og.PlanCount).HasDefaultValue(0);
        builder.Property(og => og.GraphDate).IsRequired();

        builder.HasIndex(og => og.Id).IsUnique();

        builder
            .HasOne(og => og.Configrming)
            .WithMany(o => o.ConfirmedGraphs)
            .HasForeignKey(og => og.ConfigrmingId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(og => og.Owner)
            .WithMany(o => o.OperationGraphs)
            .HasForeignKey(og => og.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(og => og.Subdivision)
            .WithMany(s => s.OperationGraphs)
            .HasForeignKey(og => og.SubdivisionId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(og => og.Status)
            .WithMany(s => s.OperationGraphs)
            .HasForeignKey(og => og.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(og => og.ProductDetail)
            .WithMany(pd => pd.OperationGraphs)
            .HasForeignKey(og => og.ProductDetailId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}