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
    public class OrderDetailConfig : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(c=>c.Id);

            builder.HasOne(c => c.ProductDetail)
                .WithMany(c => c.OrderDetails)
                .HasForeignKey(c => c.IdProductDetail)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(c=>c.Booking)
                .WithMany(c=>c.OrderDetails)
                .HasForeignKey(c=>c.IdBooking)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);  
        }
    }
}
