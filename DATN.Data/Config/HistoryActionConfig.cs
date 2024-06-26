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
    public class HistoryActionConfig : IEntityTypeConfiguration<HistoryAction>
    {
        public void Configure(EntityTypeBuilder<HistoryAction> builder)
        {

            builder.HasKey(p => p.ID);
            //
            builder.HasOne(c => c.Action)
                       .WithMany(c => c.HistoryActions)
                       .HasForeignKey(c => c.ActionID)
                       .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Booking)
                       .WithMany(c => c.HistoryActions)
                       .HasForeignKey(c => c.BookingID)
                       .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.ActionBy)
                       .WithMany(c => c.HistoryActions)
                       .HasForeignKey(c => c.ActionByID)
                       .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.ByGuest).IsRequired();
            builder.Property(p => p.Description).IsRequired();
        }

    }
}
