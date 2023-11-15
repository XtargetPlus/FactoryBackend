using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.SubdivisionInfoConfiguration.EquipmentInfoConfiguration;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.Property(e => e.Title).HasMaxLength(100);
        builder.Property(e => e.SerialNumber).HasMaxLength(50);

        builder.HasIndex(e => e.SerialNumber).IsUnique();
        builder.HasIndex(e => e.Id).IsUnique();
        builder.HasIndex(e => e.Title);

        builder
            .HasOne(e => e.Subdivision)
            .WithMany(s => s.Equipments)
            .HasForeignKey(e => e.SubdivisionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

