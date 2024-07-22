using DATN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATN.Data.Config
{
    public class WorkShiftConfig : IEntityTypeConfiguration<WorkShift>
    {
        public void Configure(EntityTypeBuilder<WorkShift> builder)
        {
            //
            builder.HasKey(x => x.Id);
            //
            builder.HasOne(c => c.Shift)
                .WithMany(c => c.WorkShifts)
                .HasForeignKey(c => c.ShiftId);

            builder.Property(c => c.WorkDate)
                .IsRequired();
        }
    }
}
