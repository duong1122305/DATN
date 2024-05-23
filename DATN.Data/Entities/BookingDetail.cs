using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.Data.Enum;
using DATN.ViewModels.Enum;

namespace DATN.Data.Entities
{
	// Bảng Chi tiết đặt lịch
	[Table("BookingDetails")]
	public class BookingDetail
	{
		[Key]
		public int Id { get; set; } // Khóa chính

		[ForeignKey("Booking")]
		public int BookingId { get; set; } // Khóa ngoại đến ID đặt lịch

		[ForeignKey("ServiceDetail")]
		public int ServiceDetailId { get; set; } // Khóa ngoại đến ID chi tiết dịch vụ

		public DateTime StartDateTime { get; set; } // Thời gian bắt đầu

		public DateTime EndDateTime { get; set; } // Thời gian kết thúc

		public BookingDetailStatus Status { get; set; } // Trạng thái đặt lịch

		public string Price { get; set; } // Giá tiền

		public virtual Booking Booking { get; set; } // Đặt lịch

		public virtual ServiceDetail ServiceDetail { get; set; } // Chi tiết dịch vụ
	}
}
