using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	// Bảng Chi tiết gói dịch vụ
	[Table("ComboDetails")]
	public class ComboDetail
	{
		[Key]
		public int Id { get; set; } // Khóa chính

		[ForeignKey("Service")]
		public int ServiceId { get; set; } // Khóa ngoại đến ID dịch vụ

		[ForeignKey("ComboService")]
		public int ComboServiceId { get; set; } // Khóa ngoại đến ID gói dịch vụ

		public string Type { get; set; } // Loại (nếu cần)

		public double Price { get; set; } // Giá tiền

		public DateTime CreateAt { get; set; } // Thời gian tạo

		public DateTime DeleteAt { get; set; } // Thời gian xóa

		public virtual Service Service { get; set; } // Dịch vụ

		public virtual ComboService ComboService { get; set; } // Gói dịch vụ
	}
}
