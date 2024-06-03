using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
    [Table("ServiceDetails")]
    public class ServiceDetail
    {
        public int Id { get; set; } // Khóa chính

        public int ServiceId { get; set; } // Khóa ngoại đến ID dịch vụ

        public string Name { get; set; } // Tên chi tiết dịch vụ

        public float Price { get; set; } // Giá tiền

        public double Duration { get; set; } // Khoảng thời gian

        public string Description { get; set; } // Mô tả
        public bool IsDeleted { get; set; } // Mô tả

        public DateTime CreateAt { get; set; } // Thời gian tạo

        public DateTime? UpdateAt { get; set; } // Thời gian cập nhật

        public DateTime? DeleteAt { get; set; } // Thời gian xóa
        public bool Deleted { get; set; } // Thời gian xóa

        public virtual Service Service { get; set; } // Dịch vụ
        public virtual ICollection<ComboDetail> ComboDetails { get; set; } // Dịch vụ
        public virtual ICollection<BookingDetail> BookingDetails { get; set; } // Dịch vụ
    }
}
