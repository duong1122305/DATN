﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	[Table("WorkShifts")]
	public class WorkShift
	{
		[Key]
		public int Id { get; set; } // Khóa chính

		public DateTime WorkDate { get; set; } // Ngày làm việc

		[ForeignKey("Shift")]
		public int ShiftId { get; set; } // Khóa ngoại đến ID ca làm

		public virtual Shift Shift { get; set; } // Ca làm

		// Quan hệ một-nhiều: Mỗi ca làm việc theo ngày có thể thuộc về nhiều lịch làm việc nhân viên
		public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set; }
	}
}