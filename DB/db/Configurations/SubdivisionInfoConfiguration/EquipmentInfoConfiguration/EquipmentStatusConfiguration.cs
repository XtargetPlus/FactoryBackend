using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.SubdivisionInfoConfiguration.EquipmentInfoConfiguration;

public class EquipmentStatusConfiguration : IEntityTypeConfiguration<EquipmentStatus>
{
    public void Configure(EntityTypeBuilder<EquipmentStatus> builder)
    {
        builder.Property(es => es.Id).IsRequired();

        builder.HasIndex(es => es.Id).IsUnique();

        builder
            .HasOne(es => es.Status)
            .WithMany(s => s.EquipmentStatuses)
            .HasForeignKey(s => s.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}