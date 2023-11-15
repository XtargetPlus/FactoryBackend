using DB.Model.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.ToolInfoConfiguration;

public class ToolParametersConcreteConfiguration : IEntityTypeConfiguration<ToolParameterConcrete>
{
    public void Configure(EntityTypeBuilder<ToolParameterConcrete> builder)
    {
        builder.HasKey(tps => new { tps.ToolParameterId, tps.ToolId });

        builder.Property(tpc => tpc.Value).HasMaxLength(255);

        builder
            .HasOne(tpc => tpc.Tool)
            .WithMany(t => t.ParametersConcrete)
            .HasForeignKey(dc => dc.ToolId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(tpc => tpc.ToolParameter)
            .WithMany(tp => tp.ParametersConcrete)
            .HasForeignKey(tpc => tpc.ToolParameterId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
