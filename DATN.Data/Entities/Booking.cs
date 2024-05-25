using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.Data.Enum;

namespace DATN.Data.Entities
{
	// Bảng Đặt lịch
	[Table("Bookings")]
	public class Booking
	{
		public int Id { get; set; } // Khóa chính

		public Guid? StaffAtCounterId { get; set; } // Khóa ngoại đến ID nhân viên thanh toán

		public Guid? StaffConfirmId { get; set; } // Khóa ngoại đến ID nhân viên xác nhận đơn đặt hàng

		public int? VoucherId { get; set; } // ID mã giảm giá (nếu có)

		public int PaymentTypeId { get; set; } // Khóa ngoại đến ID loại thanh toán

		public Guid GuestId { get; set; } // Đặt lịch cho khách hàng chưa đăng ký

		public float TotalPrice { get; set; } // Tổng giá trị đơn đặt hàng

		public DateTime BookingTime { get; set; } // Thời gian đặt lịch

		public DateTime? DesiredStartTime { get; set; } // Thời gian mong muốn bắt đầu

		public DateTime? ConfirmedTime { get; set; } // Thời gian xác nhận đơn đặt hàng

		public DateTime? CustomerArrivalTime { get; set; } // Thời gian khách hàng đến

		public DateTime? UpdatedAt { get; set; } // Thời gian cập nhật

		public string? UpdatedBy { get; set; } // Người cập nhật

		public BookingStatus Status { get; set; } // Trạng thái đơn hàng

		// Quan hệ một-nhiều: Mỗi đơn đặt hàng có thể đi kèm với nhiều chi tiết đặt lịch
		public virtual ICollection<BookingDetail> BookingDetails { get; set; }

		// Quan hệ nhiều-một: Mỗi đơn đặt hàng thuộc về một khách hàng
		public virtual User Customer { get; set; }
		public virtual Guest Guest { get; set; }
		public virtual TypePayment TypePayment { get; set; }

		// Quan hệ một-nhiều: Mỗi đơn đặt hàng có thể có nhiều thanh toán
		public virtual ICollection<Payment> Payments { get; set; }

		// Quan hệ một-nhiều: Mỗi đơn đặt hàng có thể được giảm giá bởi nhiều mã giảm giá
		public virtual Discount Discount { get; set; }

		// Quan hệ một-nhiều: Mỗi đơn đặt hàng có thể được giao bởi nhiều nhân viên
		public virtual ICollection<User> StaffAssigned { get; set; }
	}
}
