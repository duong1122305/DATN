﻿using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Brands)
                .WithOne(c => c.Product)
                .HasForeignKey<Product>(c => c.IdBrand)
                .IsRequired();

            builder.HasOne(c => c.CategoryProduct)
                .WithMany(c => c.Products)
                .HasPrincipalKey(c => c.IdCategory)
                .IsRequired();
        }
    }
}
