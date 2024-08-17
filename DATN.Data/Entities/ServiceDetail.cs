using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Data.Entities
{
    [Table("ServiceDetails")]
    public class ServiceDetail
    {
        public int Id { get; set; } // Khóa chính

        public int ServiceId { get; set; } // Khóa ngoại đến ID dịch vụ

        public float Price { get; set; } // Giá tiền

        public double Duration { get; set; } // Khoảng thời gian

        public string Description { get; set; } // Mô tả
        public float MinWeight { get; set; }
        public float MaxWeight { get; set; }
        public bool IsDeleted { get; set; } = false;

        public DateTime CreateAt { get; set; } // Thời gian tạo

        public DateTime? UpdateAt { get; set; } // Thời gian cập nhật

        public virtual Service Service { get; set; } // Dịch vụ
        public virtual ICollection<BookingDetail> BookingDetails { get; set; } // Dịch vụ
    }
}
