using DB.Model.StatusInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.StatusConfiguration;

public class TechnologicalProcessStatusConfiguration : IEntityTypeConfiguration<TechnologicalProcessStatus>
{
    public void Configure(EntityTypeBuilder<TechnologicalProcessStatus> builder)
    {
        builder.HasKey(tps => tps.Id);

        builder.Property(tps => tps.Note).HasMaxLength(1000);
        builder.Property(tps => tps.Id).IsRequired();

        builder.HasIndex(tps => tps.Id).IsUnique();

        builder
            .HasOne(tps => tps.User)
            .WithMany(u => u.TechnologicalProcessStatuses)
            .HasForeignKey(tps => tps.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(tps => tps.TechnologicalProcess)
            .WithMany(tp => tp.TechnologicalProcessStatuses)
            .HasForeignKey(tps => tps.TechnologicalProcessId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(tps => tps.Status)
            .WithMany(s => s.TechnologicalProcessStatuses)
            .HasForeignKey(tps => tps.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

