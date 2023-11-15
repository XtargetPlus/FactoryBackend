using DB.Model.AccessoryInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.AccessoryConfiguration;

public class AccessoryConfiguration : IEntityTypeConfiguration<Accessory>
{
    public void Configure(EntityTypeBuilder<Accessory> builder)
    {
        builder.Property(a => a.Description).HasMaxLength(500);
        builder.Property(a => a.TaskNumber).HasMaxLength(20);
        builder.Property(a => a.Note).HasMaxLength(500);

        builder.HasIndex(a => a.Id).IsUnique();

        builder
            .HasOne(a => a.Developer)
            .WithMany(d => d.Accessories)
            .HasForeignKey(a => a.DeveloperId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(a => a.TechnologicalProcessItem)
            .WithMany(tpi => tpi.Accessories)
            .HasForeignKey(a => a.TechnologicalProcessItemId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(a => a.OutsideOrganization)
            .WithMany(oo => oo.Accessories)
            .HasForeignKey(a => a.OutsideOrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(a => a.AccessoryType)
            .WithMany(at => at.Accessories)
            .HasForeignKey(a => a.AccessoryTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}