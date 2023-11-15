using DB.Model.StorageInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.Configurations.StorageInfoConfiguration;

public class MoveDetailConfiguration : IEntityTypeConfiguration<MoveDetail>
{
    public void Configure(EntityTypeBuilder<MoveDetail> builder)
    {
        builder.Property(md => md.Note).HasMaxLength(500);
        builder.Property(md => md.Id).IsRequired();
        builder.Property(md => md.Count).IsRequired();
        builder.Property(md => md.IsSuccess).HasDefaultValue(false);

        builder.HasIndex(md => md.Id).IsUnique();

        builder
            .HasOne(md => md.Initiator)
            .WithMany(u => u.MoveDetails)
            .HasForeignKey(md => md.InitiatorId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasMany(md => md.PreviousMovesDetails)
            .WithMany()
            .UsingEntity(
                "PreviousMovesDetails",
                l => l.HasOne(typeof(MoveDetail)).WithMany().HasForeignKey("CurrentMoveDetail"),
                r => r.HasOne(typeof(MoveDetail)).WithMany().HasForeignKey("PreviousMoveDetail"));
        builder
            .HasMany(md => md.NextMovesDetails)
            .WithMany()
            .UsingEntity(
                "NextMovesDetails",
                l => l.HasOne(typeof(MoveDetail)).WithMany().HasForeignKey("CurrentMoveDetail"),
                r => r.HasOne(typeof(MoveDetail)).WithMany().HasForeignKey("NextMoveDetail"));
        builder
            .HasOne(md => md.Storage)
            .WithMany(s => s.MoveDetails)
            .HasForeignKey(md => md.StorageId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(md => md.OperationGraphDetailItem)
            .WithMany(ogd => ogd.MoveDetails)
            .HasForeignKey(md => md.OperationGraphDetailItemId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(md => md.TechnologicalProcessItem)
            .WithMany(tpi => tpi.MoveDetails)
            .HasForeignKey(md => md.TechnologicalProcessItemId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(md => md.MoveType)
            .WithMany(mt => mt.MoveDetails)
            .HasForeignKey(md => md.MoveTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

