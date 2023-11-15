using DB.Model.TechnologicalProcessInfo.TechnologicalProcessDataInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.TechnologicalProcessInfoConfiguration;

public class TechnologicalProcessDataConfiguration : IEntityTypeConfiguration<TechnologicalProcessData>
{
    public void Configure(EntityTypeBuilder<TechnologicalProcessData> builder)
    {
        builder.Property(tpd => tpd.Rate).HasMaxLength(255);
        builder.Property(tpd => tpd.Id).IsRequired();

        builder.HasIndex(tpd => tpd.Id).IsUnique();

        builder
            .HasOne(tpd => tpd.BlankType)
            .WithMany(bt => bt.TechnologicalProcessData)
            .HasForeignKey(tpd => tpd.BlankTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(tpd => tpd.Material)
            .WithMany(m => m.TechnologicalProcessData)
            .HasForeignKey(tpd => tpd.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(tpd => tpd.TecnologicalProcess)
            .WithOne(tp => tp.TechnologicalProcessData)
            .HasForeignKey<TechnologicalProcessData>(tpd => tpd.TecnologicalProcessId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}