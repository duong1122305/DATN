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
    public class EmployeeAttendanceConfig : IEntityTypeConfiguration<EmployeeAttendance>
    {
        public void Configure(EntityTypeBuilder<EmployeeAttendance> builder)
        {
            //
            builder.HasKey(e => e.Id);
            //
            builder.HasOne(c => c.EmployeeSchedule)
                .WithMany(c => c.EmployeeAttendances)
                .HasForeignKey(c => c.EmployeeScheduleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
