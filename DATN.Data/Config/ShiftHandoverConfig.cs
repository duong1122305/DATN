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
    public class ShiftHandoverConfig : IEntityTypeConfiguration<ShiftHandover>
    {
        public void Configure(EntityTypeBuilder<ShiftHandover> builder)
        {
            //
            builder.HasKey(x => x.Id);
            //
            builder.HasOne(c => c.EmployeeAttendance)
                .WithMany(c => c.ShiftHandovers)
                .HasForeignKey(c => c.EmployeeAttendanceId);

            builder.Property(x => x.TotalOtherIncomes)
                .IsRequired();

            builder.Property(x => x.TotalOtherExpenses)
                .IsRequired();

            builder.Property(x => x.TotalEarnings)
                .IsRequired();

            builder.Property(x => x.FinalAccountBalance)
                .IsRequired();

            builder.Property(x => x.FinalCash)
                .IsRequired();

            builder.Property(x => x.InitialAccountBalance)
                .IsRequired();

            builder.Property(x => x.InitialCash)
                .IsRequired();

        }
    }
}
