using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Data.Entities
{
    [Table("WorkShifts")]
    public class WorkShift
    {
        public int Id { get; set; } // Khóa chính

        public DateTime WorkDate { get; set; } // Ngày làm việc

        public int ShiftId { get; set; } // Khóa ngoại đến ID ca làm

        public virtual Shift Shift { get; set; } // Ca làm

        // Quan hệ một-nhiều: Mỗi ca làm việc theo ngày có thể thuộc về nhiều lịch làm việc nhân viên
        public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set; }
    }
}