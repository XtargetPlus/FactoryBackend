using DB.Model.SubdivisionInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.SubdivisionInfoConfiguration;

public class SubdivisionConfiguration : IEntityTypeConfiguration<Subdivision>
{
    public void Configure(EntityTypeBuilder<Subdivision> builder)
    {
        builder.Property(s => s.Title).HasMaxLength(255).IsRequired();
        builder.Property(s => s.Id).IsRequired();

        builder.HasIndex(s => s.Id).IsUnique();
        builder.HasIndex(s => s.Title).IsUnique();

        builder
            .HasOne(s => s.Father)
            .WithMany(s => s.Subdivisions)
            .HasForeignKey(s => s.FatherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

