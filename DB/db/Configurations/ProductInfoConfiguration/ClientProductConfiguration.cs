using DB.Model.ProductInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.ProductInfoConfiguration;

public class ClientProductConfiguration : IEntityTypeConfiguration<ClientProduct>
{
    public void Configure(EntityTypeBuilder<ClientProduct> builder)
    {
        builder.HasKey(cp => new { cp.ProductId, cp.ClientId });

        builder
            .HasOne(cp => cp.Client)
            .WithMany(c => c.ClientProducts)
            .HasForeignKey(cp => cp.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(cp => cp.Product)
            .WithMany(p => p.ClientProducts)
            .HasForeignKey(cp => cp.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

