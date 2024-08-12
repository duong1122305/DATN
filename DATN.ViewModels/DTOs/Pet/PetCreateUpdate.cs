using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace DATN.ViewModels.DTOs.Pet
{
    public class PetCreateUpdate
    {
        public int Id { get; set; } // Khóa chính
        [Required(ErrorMessage = "Tạo thú cưng phải thêm chủ")]
        public Guid OwnerId { get; set; } // Khóa ngoại đến ID chủ nhân
        [Required(ErrorMessage = "Phải có loài")]

        public int SpeciesId { get; set; } // Khóa ngoại đến ID giống thú cưng
        [Required(ErrorMessage = "Phải nhập tên thú cưng")]
        public string Name { get; set; } // Tên thú cưng

        public bool Gender { get; set; } // Giới tính
       
        public DateTime? Birthday { get; set; } // Ngày sinh
        [Range(0.2, 50, ErrorMessage = "Không con pet nào cân nặng như vậy cả")]
        [Required(ErrorMessage = "Phải nhập trường này ")]
        public float? Weight { get; set; } // Cân nặng

        public bool? Neutered { get; set; } // Đã triệt sản chưa

        public string? OriginalColor { get; set; } // Màu lông ban đầu

        public bool? Vaccinated { get; set; } // Đã tiêm phòng chưa

        public string? Note { get; set; } // Ghi chú
    }
}
