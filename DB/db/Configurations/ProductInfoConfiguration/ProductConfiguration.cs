using DB.Model.ProductInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.db.ProductInfoConfiguration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {   
        builder.HasIndex(p => p.Id).IsUnique();
        builder.HasIndex(p => p.Price);

        builder
            .HasOne(p => p.Detail)
            .WithMany(d => d.Products)
            .HasForeignKey(p => p.DetailId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

