using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	// Bảng Khách không có tài khoản
	[Table("Guests")]
	public class Guest
	{
		[Key]
		public int Id { get; set; } // Khóa chính

		public Guid Guid { get; set; } // Guid khách hàng

		[Required]
		public string Name { get; set; } // Tên khách hàng

		public bool Gender { get; set; } // Giới tính

		[Required]
		public string PhoneNumber { get; set; } // Số điện thoại

		public DateTime RegisteredAt { get; set; } // Thời gian đăng ký
	}
}
