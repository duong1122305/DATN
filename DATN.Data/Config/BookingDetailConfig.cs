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
    public class BookingDetailConfig : IEntityTypeConfiguration<BookingDetail>
    {
        public void Configure(EntityTypeBuilder<BookingDetail> builder)
        {
            //
            builder.HasKey(x => x.Id);
            //
            builder.HasOne(c => c.Booking)
                .WithMany(c => c.BookingDetails)
                .HasForeignKey(c => c.BookingId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            //
            builder.HasOne(c=>c.ServiceDetail)
                .WithMany(c=>c.BookingDetails)
                .HasForeignKey(c=>c.ServiceDetailId);
            //
            builder.HasOne(c => c.ComboService)
                .WithMany(c => c.BookingDetails)
                .HasForeignKey(c => c.ComboId);
            //
            builder.HasOne(c => c.Staff)
                 .WithMany(c => c.BookingDetails)
                 .HasForeignKey(c => c.StaffId)
                 .IsRequired();
            //
            builder.Property(c=>c.Price)
                .IsRequired();
            //
            builder.Property(c=>c.Status)
                .IsRequired();
            //
            builder.Property(c=>c.StartDateTime)
                .IsRequired();
            //
            builder.Property(c=>c.EndDateTime)
                .IsRequired();
        }
    }
}
