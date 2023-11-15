using DB.Model.AccessoryInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.AccessoryConfiguration;

public class AccessoryEquipmentConfiguration : IEntityTypeConfiguration<AccessoryEquipment>
{
    public void Configure(EntityTypeBuilder<AccessoryEquipment> builder)
    {
        builder.HasKey(ae => new { ae.EquipmentId, ae.AccessoryId });

        builder
            .HasOne(ae => ae.Equipment)
            .WithMany(e => e.AccessoryEquipments)
            .HasForeignKey(e => e.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(ae => ae.Accessory)
            .WithMany(a => a.AccessoryEquipments)
            .HasForeignKey(e => e.AccessoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

