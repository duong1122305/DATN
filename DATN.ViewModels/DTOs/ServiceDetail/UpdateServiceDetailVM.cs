using System.ComponentModel.DataAnnotations;

namespace DATN.ViewModels.DTOs.ServiceDetail
{
    public class UpdateServiceDetailVM
    {
        [Required(ErrorMessage = "Chưa chọn dịch vụ")]
        public int ServiceId { get; set; } // Khóa ngoại đến ID dịch vụ
        [Range(1000.0f, 5000000.0f, ErrorMessage = "Giá phải trong khoảng 1.000vnđ đến 5.000.000vnđ")]
        public float Price { get; set; }

        [Range(1, 90, ErrorMessage = "Thời gian làm trong khoảng 1-90 phút")]
        public double Duration { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string Description { get; set; }

       
    }
}
