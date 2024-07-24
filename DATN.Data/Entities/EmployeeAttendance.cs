using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Data.Entities
{
    // Bảng Điểm danh nhân viên
    [Table("EmployeeAttendances")]
    public class EmployeeAttendance
    {
        public int Id { get; set; } // Khóa chính

        public int EmployeeScheduleId { get; set; } // Khóa ngoại đến ID lịch làm việc

        public DateTime? CheckInTime { get; set; } // Thời gian vào

        public DateTime? CheckOutTime { get; set; } // Thời gian ra


        public string? OtherNotes { get; set; } // Ghi chú khác

        public virtual EmployeeSchedule EmployeeSchedule { get; set; } // Lịch làm việcB
    }
}
