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
    public class ShiftConfig : IEntityTypeConfiguration<Shift>
    {
        public void Configure(EntityTypeBuilder<Shift> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c=>c.Name)
                .IsRequired();

            builder.Property(c=>c.From)
                .IsRequired();

            builder.Property(c=>c.To)
                .IsRequired();
        }
    }
}
