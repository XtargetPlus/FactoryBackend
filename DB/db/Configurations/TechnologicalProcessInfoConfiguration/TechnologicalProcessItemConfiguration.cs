using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.TechnologicalProcessInfoConfiguration;

public class TechnologicalProcessItemConfiguration : IEntityTypeConfiguration<TechnologicalProcessItem>
{
    public void Configure(EntityTypeBuilder<TechnologicalProcessItem> builder)
    {
        builder.Property(tpi => tpi.OperationNumber).HasMaxLength(255);
        builder.Property(tpi => tpi.Priority).HasColumnType("int unsigned");
        builder.Property(tpi => tpi.Show).HasColumnType("tinyint(1)");
        builder.Property(tpi => tpi.Note).HasMaxLength(255);

        builder.HasQueryFilter(p => p.Show);

        builder.HasIndex(tpi => tpi.Id).IsUnique();

        builder
            .HasOne(tpi => tpi.Operation)
            .WithMany(o => o.TechnologicalProcessItems)
            .HasForeignKey(tpi => tpi.OperationId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(tpi => tpi.TechnologicalProcess)
            .WithMany(tp => tp.TechnologicalProcessItems)
            .HasForeignKey(tpi => tpi.TechnologicalProcessId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(tpi => tpi.MainTechnologicalProcessItem)
            .WithMany(mtpi => mtpi.BranchesTechnologicalProcessItems)
            .HasForeignKey(tpi => tpi.MainTechnologicalProcessItemId) 
            .OnDelete(DeleteBehavior.Restrict);
    }
}