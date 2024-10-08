﻿using DATN.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Data.Entities
{
    // Bảng Đặt lịch
    [Table("Bookings")]
    public class Booking
    {
        public int Id { get; set; } // Khóa chính
        public bool IsPayment { get; set; }
        public int? VoucherId { get; set; } // ID mã giảm giá (nếu có)
        public double? ReducedAmount { get; set; } // số tiền giảm

        public int? PaymentTypeId { get; set; } // Khóa ngoại đến ID loại thanh toán

        public Guid GuestId { get; set; } // Đặt lịch cho khách hàng chưa đăng ký

        public double TotalPrice { get; set; } // Tổng giá trị đơn đặt hàng

        public DateTime BookingTime { get; set; } // Thời gian đặt lịch

        //     public DateTime? AppointmentTime { get; set; } // Thời gian mong muốn bắt đầu

        //public DateTime? ConfirmedTime { get; set; } // Thời gian xác nhận đơn đặt hàng

        //public DateTime? CustomerArrivalTime { get; set; } // Thời gian khách hàng đến

        //public DateTime? UpdatedAt { get; set; } // Thời gian cập nhật

        //public Guid? UpdatedBy { get; set; } // Người cập nhật
        public bool IsAddToSchedule { get; set; }// thêm vào bảng
        public BookingStatus Status { get; set; } // Trạng thái đơn hàng

        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual ICollection<HistoryAction> HistoryActions { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Report> Reports { get; set; }


        public virtual Guest Guest { get; set; }
        public virtual TypePayment TypePayment { get; set; }
        public virtual Discount Discount { get; set; }

    }
}
