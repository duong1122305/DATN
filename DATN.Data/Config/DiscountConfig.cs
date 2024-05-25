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
    public class DiscountConfig : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(c=>c.VoucherCode)
                .IsUnique();

            builder.Property(c => c.Description)
                .IsRequired();

            builder.Property(c => c.Created)
                .IsRequired();

            builder.Property(c => c.StartDate)
                .IsRequired();

            builder.Property(c => c.EndDate)
                .IsRequired();

            builder.Property(c => c.DiscountPercent)
                .IsRequired();

            builder.Property(c => c.MaxMoneyDiscount)
                .IsRequired();

            builder.Property(c => c.MinMoneyApplicable)
                .IsRequired();
        }
    }
}
