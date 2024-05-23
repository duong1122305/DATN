using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	[Table("Shift")]// Ca làm việc
	public class Shift
	{
		[Key]
		public int Id { get; set; } // PK

		[Required]
		public string Name { get; set; } // Tên hoặc mã của ca làm việc

		[Required]
		public TimeSpan From { get; set; } // Thời gian bắt đầu ca

		[Required]
		public TimeSpan To { get; set; } // Thời gian kết thúc ca

		// Navigation property
		public virtual ICollection<WorkShift> WorkShifts { get; set; }
	}
}