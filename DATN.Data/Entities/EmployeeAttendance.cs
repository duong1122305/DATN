using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.ViewModels.Enum;

namespace DATN.Data.Entities
{
	// Bảng Điểm danh nhân viên
	[Table("EmployeeAttendances")]
	public class EmployeeAttendance
	{
		public int Id { get; set; } // Khóa chính

		public int EmployeeScheduleId { get; set; } // Khóa ngoại đến ID lịch làm việc

		public Guid UserId { get; set; } // Khóa ngoại đến ID nhân viên

		public DateTime? CheckInTime { get; set; } // Thời gian vào

		public DateTime? CheckOutTime { get; set; } // Thời gian ra


		public string? OtherNotes { get; set; } // Ghi chú khác

		public virtual EmployeeSchedule EmployeeSchedule { get; set; } // Lịch làm việc

		public virtual User User { get; set; } // Nhân viên
	}
}
