using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Data.Entities
{
    // Bảng Báo cáo
    [Table("Reports")]
    public class Report
    {
        public int Id { get; set; } // Khóa chính

        public int BookingId { get; set; } // Khóa ngoại đến ID chi tiết dịch vụ

        public string Comment { get; set; } // Bình luận

        [Range(1, 5, ErrorMessage = "Số sao chỉ từ 1 - 5")]
        public int Rate { get; set; }

        public DateTime CreateAt { get; set; } // Thời gian tạo

        // Quan hệ một-nhiều: Mỗi báo cáo liên quan đến một chi tiết dịch vụ
        public virtual BookingDetail BookingDetail { get; set; }
    }
}
