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
        public int Id { get; set; } // Khóa chính

        public int BookingId { get; set; } // Khóa ngoại đến ID đặt lịch

        public Guid? StaffId { get; set; } // Khóa ngoại đến ID đặt lịch

        public int? PetId { get; set; } = 1;

        public int? ComboId { get; set; }

        public int? ServiceDetailId { get; set; } // Khóa ngoại đến ID chi tiết dịch vụ

        public DateTime StartDateTime { get; set; } // Thời gian bắt đầu

        public DateTime EndDateTime { get; set; } // Thời gian kết thúc

        public BookingDetailStatus Status { get; set; } // Trạng thái đặt lịch

        public double Price { get; set; } // Giá tiền
        public int Quantity { get; set; } // Số lượng

        public virtual Booking Booking { get; set; } // Đặt lịch
        public virtual Pet Pet { get; set; }
        public virtual ComboService ComboService { get; set; } // Đặt lịch
        public virtual User Staff { get; set; } // Đặt lịch
        public virtual ICollection<Report> Reports { get; set; }

        public virtual ServiceDetail ServiceDetail { get; set; } // Chi tiết dịch vụ
    }
}
