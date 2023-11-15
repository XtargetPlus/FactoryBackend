using DB.Model.UserInfo.RoleInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.UserInfoConfiguration.RoleInfoConfiguration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(r => r.Title).HasMaxLength(100).IsRequired();
        builder.Property(r => r.Id).IsRequired();

        builder.HasIndex(r => r.Id).IsUnique();
        builder.HasIndex(r => r.Title).IsUnique();
    }
}