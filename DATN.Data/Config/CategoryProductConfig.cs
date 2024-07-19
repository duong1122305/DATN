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
    public class CategoryProductConfig : IEntityTypeConfiguration<CategoryDetails>
    {
        public void Configure(EntityTypeBuilder<CategoryDetails> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Category)
                .WithMany(c => c.CategoryProducts)
                .HasForeignKey(c => c.IdCategory)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
