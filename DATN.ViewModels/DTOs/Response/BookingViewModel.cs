using DATN.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Response
{
    public class BookingViewModel
    {
		public int Id { get; set; } // Khóa chính

		public Guid? StaffAtCounterId { get; set; } // Khóa ngoại đến ID nhân viên thanh toán
		public string? StaffCounterName {  get; set; }

		public Guid? StaffConfirmId { get; set; } // Khóa ngoại đến ID nhân viên xác nhận đơn đặt hàng

		public string? StaffConfirmName{ get; set; }//tên nhân viên xác nhận

		public int? VoucherId { get; set; } // ID mã giảm giá (nếu có)
		public double? ReducedAmount { get; set; } // số tiền giảm

		public int PaymentTypeId { get; set; } // Khóa ngoại đến ID loại thanh toán
		public string PaymentName { get; set; } // Tên loại thanh toán
		
		public Guid GuestId { get; set; } // Đặt lịch cho khách hàng chưa đăng ký
        public string GuestName { get; set; }

        public float TotalPrice { get; set; } // Tổng giá trị đơn đặt hàng

		public DateTime BookingTime { get; set; } // Thời gian đặt lịch

		public DateTime? DesiredStartTime { get; set; } // Thời gian mong muốn bắt đầu

		public DateTime? ConfirmedTime { get; set; } // Thời gian xác nhận đơn đặt hàng

		public DateTime? CustomerArrivalTime { get; set; } // Thời gian khách hàng đến

		public DateTime? UpdatedAt { get; set; } // Thời gian cập nhật

		public Guid? UpdatedBy { get; set; } // Người cập nhật
        public  string UpdateByName { get; set; }

        public BookingStatus Status { get; set; } // Trạng thái đơn hàng
	}
}
