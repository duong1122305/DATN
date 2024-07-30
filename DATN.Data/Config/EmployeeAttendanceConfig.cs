using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
