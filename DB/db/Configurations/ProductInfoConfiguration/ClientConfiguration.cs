using DB.Model.ProductInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DB.db.ProductInfoConfiguration;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.Property(c => c.Title).HasMaxLength(255);

        builder.HasIndex(c => c.Title).IsUnique();
        builder.HasIndex(c => c.Id).IsUnique();
    }
}

