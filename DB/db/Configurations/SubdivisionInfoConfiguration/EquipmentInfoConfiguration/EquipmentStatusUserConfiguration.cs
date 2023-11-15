using DB.Model.SubdivisionInfo.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.SubdivisionInfoConfiguration.EquipmentInfoConfiguration;

public class EquipmentStatusUserConfiguration : IEntityTypeConfiguration<EquipmentStatusUser>
{
    public void Configure(EntityTypeBuilder<EquipmentStatusUser> builder)
    {
        builder.HasKey(esu => new { esu.UserId, esu.EquipmentStatusId });

        builder
            .HasOne(esu => esu.User)
            .WithMany(u => u.EquipmentStatusUsers)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(esu => esu.EquipmentStatus)
            .WithMany(es => es.EquipmentStatusUsers)
            .HasForeignKey(esu => esu.EquipmentStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}