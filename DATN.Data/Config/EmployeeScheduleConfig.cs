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
    public class EmployeeScheduleConfig : IEntityTypeConfiguration<EmployeeSchedule>
    {
        public void Configure(EntityTypeBuilder<EmployeeSchedule> builder)
        {
            //
            builder.HasKey(e => e.Id);
            //
            builder.HasOne(c => c.User)
                .WithMany(c => c.EmployeeSchedules)
                .HasForeignKey(c => c.UserId);
            //
            builder.HasOne(c => c.WorkShift)
                .WithMany(c => c.EmployeeSchedules)
                .HasForeignKey(c => c.WorkShiftId);

        }
    }
}
