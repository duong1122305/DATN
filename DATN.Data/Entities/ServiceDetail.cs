using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	[Table("ServiceDetails")]
	public class ServiceDetail
	{
		[Key]
		public int Id { get; set; } // Khóa chính

		[ForeignKey("Service")]
		public int ServiceId { get; set; } // Khóa ngoại đến ID dịch vụ

		[Required]
		public string Name { get; set; } // Tên chi tiết dịch vụ

		public float Price { get; set; } // Giá tiền

		public double Duration { get; set; } // Khoảng thời gian

		public string Description { get; set; } // Mô tả

		public DateTime CreateAt { get; set; } // Thời gian tạo

		public DateTime UpdateAt { get; set; } // Thời gian cập nhật

		public DateTime DeleteAt { get; set; } // Thời gian xóa

		public virtual Service Service { get; set; } // Dịch vụ
	}
}
