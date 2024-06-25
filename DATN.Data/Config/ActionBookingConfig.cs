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
    public class ActionBookingConfig : IEntityTypeConfiguration<ActionBooking>
    {
        public void Configure(EntityTypeBuilder<ActionBooking> builder)
        {
            builder.HasKey(p=>p.ID);
            builder.Property(p=>p.Name).IsRequired();
        }
    }
}
