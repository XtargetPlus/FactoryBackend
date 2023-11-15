using DB.Model.UserInfo.RoleInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.UserInfoConfiguration.RoleInfoConfiguration;

public class UserFormConfiguration : IEntityTypeConfiguration<UserForm>
{
    public void Configure(EntityTypeBuilder<UserForm> builder)
    {
        builder.Property(uf => uf.Title).HasMaxLength(100).IsRequired();
        builder.Property(uf => uf.Id).IsRequired();

        builder.HasIndex(uf => uf.Id).IsUnique();
        builder.HasIndex(uf => uf.Title).IsUnique();
    }
}