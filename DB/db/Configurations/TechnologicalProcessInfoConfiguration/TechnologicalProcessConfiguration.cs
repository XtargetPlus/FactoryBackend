using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.TechnologicalProcessInfoConfiguration;

public class TechnologicalProcessConfiguration : IEntityTypeConfiguration<TechnologicalProcess>
{
    public void Configure(EntityTypeBuilder<TechnologicalProcess> builder)
    {
        builder.Property(tp => tp.Note).HasMaxLength(1000);
        builder.Property(tp => tp.Id).IsRequired();

        builder.HasIndex(tp => tp.Id).IsUnique();

        builder.HasQueryFilter(tp => tp.IsActual);

        builder
            .HasOne(tp => tp.Detail)
            .WithMany(d => d.TechnologicalProcesses)
            .HasForeignKey(tp => tp.DetailId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(tp => tp.Developer)
            .WithMany(d => d.TechnologicalProcesses)
            .HasForeignKey(tp => tp.DeveloperId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}