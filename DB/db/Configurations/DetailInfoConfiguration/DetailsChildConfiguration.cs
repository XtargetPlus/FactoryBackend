using DB.Model.DetailInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DB.db.DetailInfoConfiguration;

public class DetailsChildConfiguration : IEntityTypeConfiguration<DetailsChild>
{
    public void Configure(EntityTypeBuilder<DetailsChild> builder)
    {
        builder.HasKey(dc => new { dc.FatherId, dc.ChildId });

        builder.HasIndex(dc => dc.Count);
        builder.HasIndex(dc => dc.Number);

        builder
            .HasOne(dc => dc.Child)
            .WithMany(c => c.DetailsFathers)
            .HasForeignKey(dc => dc.ChildId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(dc => dc.Father)
            .WithMany(f => f.DetailsChildren)
            .HasForeignKey(dc => dc.FatherId)
            .OnDelete(DeleteBehavior.Restrict);  
    }
}

