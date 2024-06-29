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
    public class ServiceDetailConfig : IEntityTypeConfiguration<ServiceDetail>
    {
        public void Configure(EntityTypeBuilder<ServiceDetail> builder)
        {
            //
            builder.HasKey(x => x.Id);
            //
            builder.HasOne(c => c.Service)
                .WithMany(c => c.ServiceDetails)
                .HasForeignKey(c => c.ServiceId);

            builder.Property(x => x.Price)
                .IsRequired();

            builder.Property(x => x.Duration)
                .IsRequired();

            builder.Property(x => x.CreateAt)
                .IsRequired();

            builder.Property(c => c.MinWeight)
                .IsRequired();

            builder.Property(c => c.MaxWeight)
                .IsRequired();
        }
    }
}
