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
    public class BookingConfig : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            //
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Discount)
                .WithMany(c => c.Booking)
                .HasForeignKey(c => c.VoucherId);
            //
            builder.HasOne(c => c.TypePayment)
                .WithMany(c => c.Bookings)
                .HasForeignKey(c => c.PaymentTypeId)
                .IsRequired();
            //
            builder.HasOne(c => c.Guest)
                .WithMany(c => c.Bookings)
                .HasForeignKey(c => c.GuestId)
                .IsRequired();
            //
            builder.Property(c => c.TotalPrice)
                .IsRequired();
            //
            builder.Property(c => c.BookingTime)
                .IsRequired();
            //
            builder.Property(c => c.Status)
                .IsRequired();
        }
    }
}
