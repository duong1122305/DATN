using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN.Data.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Brands)
                .WithMany(c => c.Products)
                .HasForeignKey(c => c.IdBrand)
                .IsRequired();

            builder.HasOne(c => c.CategoryDetails)
                .WithMany(c => c.Products)
                .HasForeignKey(c => c.IdCategoryDeatail)
                .IsRequired();
        }
    }
}
