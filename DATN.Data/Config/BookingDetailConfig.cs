using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            builder.HasOne(c => c.ServiceDetail)
                .WithMany(c => c.BookingDetails)
                .HasForeignKey(c => c.ServiceDetailId);
            //

            builder.HasOne(c => c.Pet)
                .WithMany(c => c.BookingDetails)
                .HasForeignKey(c => c.PetId)
                .OnDelete(DeleteBehavior.Restrict);
            //
            builder.HasOne(c => c.Staff)
                 .WithMany(c => c.BookingDetails)
                 .HasForeignKey(c => c.StaffId)
                 .IsRequired();
            //
            builder.Property(c => c.Price)
                .IsRequired();
            //
            builder.Property(c => c.Status)
                .IsRequired();
            //
            builder.Property(c => c.StartDateTime)
                .IsRequired();
            //
            builder.Property(c => c.EndDateTime)
                .IsRequired();
        }
    }
}
