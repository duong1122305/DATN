using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Data.Entities
{
    // Bảng Lịch làm việc nhân viên
    [Table("EmployeeSchedules")]
    public class EmployeeSchedule
    {
        public int Id { get; set; } // Khóa chính

        public int WorkShiftId { get; set; } // Khóa ngoại đến ID ca làm việc theo ngày

        public Guid UserId { get; set; } // Khóa ngoại đến ID nhân viên


        public virtual WorkShift WorkShift { get; set; } // Ca làm việc theo ngày

        public virtual User User { get; set; } // Nhân viên

        // Quan hệ một-nhiều: Mỗi lịch làm việc nhân viên có thể đi kèm với nhiều điểm danh nhân viên
        public virtual ICollection<EmployeeAttendance> EmployeeAttendances { get; set; }
        public virtual ICollection<WorkShift> WorkShifts { get; set; }
    }
}
