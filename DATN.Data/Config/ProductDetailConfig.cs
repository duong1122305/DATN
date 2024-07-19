using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
