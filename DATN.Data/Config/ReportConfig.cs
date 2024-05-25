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
    public class ReportConfig : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            //
            builder.HasKey(x => x.Id);
            //
            builder.HasOne(c => c.BookingDetail)
                .WithMany(c => c.Reports)
                .HasForeignKey(c => c.BookingDetailId);

            builder.Property(x => x.Comment)
                .IsRequired();

            builder.Property(c=>c.CreateAt)
                .IsRequired();
        }
    }
}
