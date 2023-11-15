using DB.Model.StatusInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.StatusConfiguration;

public class AccessoryDevelopmentStatusConfiguration : IEntityTypeConfiguration<AccessoryDevelopmentStatus>
{
    public void Configure(EntityTypeBuilder<AccessoryDevelopmentStatus> builder)
    {
        builder.HasKey(ads => new { ads.UserId, ads.StatusId, ads.AccessoryId });
        builder.Property(ads => ads.DocumentNumber).HasMaxLength(50);

        builder
            .HasOne(ads => ads.User)
            .WithMany(u => u.AccessoryDevelopmentStatuses)
            .HasForeignKey(ads => ads.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(ads => ads.Accessory)
            .WithMany(a => a.AccessoryDevelopmentStatuses)
            .HasForeignKey(ads => ads.AccessoryId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(ads => ads.Status)
            .WithMany(s => s.AccessoryDevelopmentStatuses)
            .HasForeignKey(ads => ads.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

