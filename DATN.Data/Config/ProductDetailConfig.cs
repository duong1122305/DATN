using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN.Data.Config
{
    public class ProductDetailConfig : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(c => c.Product)
                .WithMany(c => c.ProductDetails)
                .HasForeignKey(c => c.IdProduct)
                .IsRequired();
        }
    }
}
