using DB.Model.StorageInfo.Graph;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.StorageInfoConfiguration;

public class OperationGraphUserConfiguration : IEntityTypeConfiguration<OperationGraphUser>
{
    public void Configure(EntityTypeBuilder<OperationGraphUser> builder)
    {
        builder.HasKey(ogu => new { ogu.OperationGraphId, ogu.UserId });

        builder
            .HasOne(ogu => ogu.User)
            .WithMany(u => u.OperationGraphUsers)
            .HasForeignKey(ogu => ogu.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(ogu => ogu.OperationGraph)
            .WithMany(og => og.OperationGraphUsers)
            .HasForeignKey(ogu => ogu.OperationGraphId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}