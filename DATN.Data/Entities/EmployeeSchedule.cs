using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	// Bảng Lịch làm việc nhân viên
	[Table("EmployeeSchedules")]
	public class EmployeeSchedule
	{
		[Key]
		public int Id { get; set; } // Khóa chính

		[ForeignKey("WorkShift")]
		public int WorkShiftId { get; set; } // Khóa ngoại đến ID ca làm việc theo ngày

		[ForeignKey("User")]
		public string UserId { get; set; } // Khóa ngoại đến ID nhân viên


		public virtual WorkShift WorkShift { get; set; } // Ca làm việc theo ngày

		public virtual User User { get; set; } // Nhân viên

		// Quan hệ một-nhiều: Mỗi lịch làm việc nhân viên có thể đi kèm với nhiều điểm danh nhân viên
		public virtual ICollection<EmployeeAttendance> EmployeeAttendances { get; set; }
	}
}
