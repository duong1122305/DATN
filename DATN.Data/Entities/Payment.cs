using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	// Bảng Thanh toán
	[Table("Payments")]
	public class Payment
	{
		[Key]
		public int Id { get; set; } // Khóa chính

		[ForeignKey("Booking")]
		public int BookingId { get; set; } // Khóa ngoại đến ID đặt lịch

		public float Amount { get; set; } // Số tiền thanh toán

		public DateTime PaymentTime { get; set; } // Thời gian thanh toán

		public virtual Booking Booking { get; set; } // Đặt lịch
	}
}
