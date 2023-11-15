using DB.Model.UserInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.UserInfoConfiguration.RoleInfoConfiguration;

public class RoleClientConfiguration : IEntityTypeConfiguration<RoleClient>
{
    public void Configure(EntityTypeBuilder<RoleClient> builder)
    {
        builder.HasKey(rc => new { rc.RoleId, rc.UserFormId });

        builder
            .HasOne(rc => rc.Role)
            .WithMany(r => r.RoleClients)
            .HasForeignKey(rc => rc.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(rc => rc.UserForm)
            .WithMany(cf => cf.RoleClients)
            .HasForeignKey(rc => rc.UserFormId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}