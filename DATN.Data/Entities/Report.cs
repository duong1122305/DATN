using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Data.Entities
{
    // Bảng Báo cáo
    [Table("Reports")]
    public class Report
    {
        public int Id { get; set; } // Khóa chính

        public int BookingDetailId { get; set; } // Khóa ngoại đến ID chi tiết dịch vụ

        public string Comment { get; set; } // Bình luận

        public DateTime CreateAt { get; set; } // Thời gian tạo

        // Quan hệ một-nhiều: Mỗi báo cáo liên quan đến một chi tiết dịch vụ
        public virtual BookingDetail BookingDetail { get; set; }
    }
}
