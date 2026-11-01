using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportDesk.Data.Entities;

namespace SupportDesk.Data.Configuration;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> b)
    {
        b.ToTable("Products");
        b.HasKey(x => x.ProductId);
        b.Property(x => x.Sku).HasMaxLength(50);
        b.Property(x => x.Name).HasMaxLength(200);
        b.Property(x => x.Category).HasMaxLength(100);
    }
}
