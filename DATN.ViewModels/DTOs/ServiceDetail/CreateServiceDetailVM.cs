using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.ServiceDetail
{
    public class CreateServiceDetailVM
    {
        public int ServiceId { get; set; } // Khóa ngoại đến ID dịch vụ

        public string Name { get; set; } // Tên chi tiết dịch vụ

        public float Price { get; set; } // Giá tiền

        public double Duration { get; set; } // Khoảng thời gian

        public string Description { get; set; } // Mô tả
        public bool IsDeleted { get; set; } = false; // Mô tả

        public DateTime CreateAt { get; set; } // Thời gian tạo
    }
}
