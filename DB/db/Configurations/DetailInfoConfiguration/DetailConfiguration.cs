using DB.Model.DetailInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.DetailInfoConfiguration;

public class DetailConfiguration : IEntityTypeConfiguration<Detail>
{
    public void Configure(EntityTypeBuilder<Detail> builder)
    {
        builder.Property(d => d.SerialNumber).HasMaxLength(100);
        builder.Property(d => d.Title).HasMaxLength(255);
        builder.Property(d => d.UnitId).HasDefaultValue(1);
        
        builder.HasIndex(d => d.SerialNumber).IsUnique();
        builder.HasIndex(d => d.Title);
        builder.HasIndex(d => d.Id).IsUnique();

        builder
            .HasOne(d => d.DetailType)
            .WithMany(dt => dt.Details)
            .HasForeignKey(d => d.DetailTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(d => d.Unit)
            .WithMany(u => u.Details)
            .HasForeignKey(d => d.UnitId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

