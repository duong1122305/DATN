using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN.Data.Config
{
    public class ReportConfig : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            //
            builder.HasKey(x => x.Id);
            //
            builder.HasOne(c => c.Booking)
                .WithMany(c => c.Reports)
                .HasForeignKey(c => c.BookingId);

            builder.Property(x => x.Comment)
                .IsRequired();

            builder.Property(c => c.CreateAt)
                .IsRequired();
        }
    }
}
