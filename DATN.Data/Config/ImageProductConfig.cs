using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN.Data.Config
{
    public class ImageProductConfig : IEntityTypeConfiguration<ImageProduct>
    {
        public void Configure(EntityTypeBuilder<ImageProduct> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(c => c.Product)
                .WithMany(c => c.ImageProducts)
                .HasForeignKey(c => c.ProductID)
                .IsRequired();
        }
    }
}
