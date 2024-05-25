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
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            //
            builder.HasKey(x => x.Id);
            //
            builder.HasOne(c => c.TypePayment)
                .WithMany(c => c.Payment)
                .HasForeignKey(c => c.IdTypePayment);

            builder.Property(x => x.Amount)
                .IsRequired();
        }
    }
}
