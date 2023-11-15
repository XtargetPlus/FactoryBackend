using DB.Model.UserInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.UserInfoConfiguration;

public class ProfessionConfiguration : IEntityTypeConfiguration<Profession>
{
    public void Configure(EntityTypeBuilder<Profession> builder)
    {
        builder.Property(p => p.Title).HasMaxLength(255);

        builder.HasIndex(p => p.Title).IsUnique();
        builder.HasIndex(p => p.Id).IsUnique();
    }
}