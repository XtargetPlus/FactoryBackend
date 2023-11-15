using DB.Model.UserInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.UserInfoConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(50).IsRequired();
        builder.Property(u => u.FathersName).HasMaxLength(50).IsRequired();
        builder.Property(u => u.FFL).HasMaxLength(50).IsRequired();
        builder.Property(u => u.Password).HasMaxLength(20).HasDefaultValue("qwerty");
        builder.Property(u => u.ProfessionNumber).HasMaxLength(10).IsRequired();
        builder.Property(u => u.Id).IsRequired();

        builder.HasIndex(u => u.ProfessionNumber).IsUnique();
        builder.HasIndex(u => u.Id).IsUnique();

        builder
            .HasOne(u => u.Profession)
            .WithMany(p => p.Users)
            .HasForeignKey(u => u.ProfessionId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(u => u.Subdivision)
            .WithMany(s => s.Users)
            .HasForeignKey(u => u.SubdivisionId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(u => u.Status)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
