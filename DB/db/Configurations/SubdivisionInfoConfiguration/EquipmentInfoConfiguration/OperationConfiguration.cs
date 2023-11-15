using DB.Model.TechnologicalProcessInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.SubdivisionInfoConfiguration.EquipmentInfoConfiguration;

public class OperationConfiguration : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.Property(o => o.FullName).HasMaxLength(255).IsRequired();
        builder.Property(o => o.ShortName).HasMaxLength(100).IsRequired();
        builder.Property(o => o.Id).IsRequired();

        builder.HasIndex(o => o.Id).IsUnique();
        builder.HasIndex(o => o.FullName).IsUnique();
        builder.HasIndex(o => o.ShortName);
    }
}