using System.ComponentModel.DataAnnotations;

namespace DATN.ViewModels.DTOs.ServiceDetail
{
    public class CreateServiceDetailVM
    {
        [Required(ErrorMessage = "Chưa chọn dịch vụ")]
        public int ServiceId { get; set; } // Khóa ngoại đến ID dịch vụ

        //[Required(ErrorMessage = "Bắt buộc nhập tên dịch vụ chi tiết")]
        //[MaxLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        //[MinLength(5, ErrorMessage = "Tối thiểu 5 ký tự")]
        //public string Name { get; set; } // Tên chi tiết dịch vụ

        [Range(1000, 10000000, ErrorMessage = "Giá phải từ 1.000 đến 10,000,000 vnđ")]
        public float Price { get; set; } // Giá tiền

        [Range(1, 90, ErrorMessage = "Thời gian phải từ 1-90 phút")]
        public double Duration { get; set; } // Khoảng thời gian

        [Required(ErrorMessage = "Tên dịch vụ chi tiết không được để trống")]
        public string Description { get; set; } // Mô tả
        public bool IsDeleted { get; set; } = false; // Mô tả

        public DateTime CreateAt { get; set; } // Thời gian tạo
    }
}
