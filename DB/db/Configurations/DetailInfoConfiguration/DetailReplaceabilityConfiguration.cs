using DB.Model.DetailInfo;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DB.db.Configurations.DetailInfoConfiguration;

public class DetailReplaceabilityConfiguration : IEntityTypeConfiguration<DetailReplaceability>
{
    public void Configure(EntityTypeBuilder<DetailReplaceability> builder)
    {
        builder.HasKey(dr => new { dr.FirstDetailId, dr.SecondDetailId});

        builder
            .HasOne(dr => dr.FirstDetail)
            .WithMany(d => d.Ins)
            .HasForeignKey(dr => dr.FirstDetailId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(dr => dr.SecondDetail)
            .WithMany(f => f.Outs)
            .HasForeignKey(dr => dr.SecondDetailId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

