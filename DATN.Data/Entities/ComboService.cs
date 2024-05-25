using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	// Bảng Gói dịch vụ
	[Table("ComboServices")]
	public class ComboService
	{
		public int Id { get; set; } // Khóa chính

		public string Name { get; set; } // Tên gói dịch vụ

		public DateTime CreateAt { get; set; } // Thời gian tạo

		public string Description { get; set; } // Mô tả

		public double Price { get; set; } // Giá tiền

		public DateTime DeleteAt { get; set; } // Thời gian xóa
		public bool IsDeleted { get; set; } // Thời gian xóa

		public virtual PetType PetType { get; set; } // Loại thú cưng
		public virtual ComboDetail ComboDetail { get; set; } // Loại thú cưng
		public virtual ICollection<BookingDetail> BookingDetails { get; set; } // Loại thú cưng
	}
}
