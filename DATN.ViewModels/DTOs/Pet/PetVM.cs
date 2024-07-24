namespace DATN.ViewModels.DTOs.Pet
{
    public class PetVM
    {
        public int Id { get; set; } // Khóa chính
        public Guid OwnerId { get; set; } // Khóa ngoại đến ID chủ nhân
        public string? Owner { get; set; } // Khóa ngoại đến ID chủ nhân
        public int SpeciesId { get; set; } // Khóa ngoại đến ID giống thú cưng
        public string? Species { get; set; } // Khóa ngoại đến ID giống thú cưng
        public string Name { get; set; } // Tên thú cưng
        public bool Gender { get; set; } // Giới tính
        public int? PetTypeId { get; set; }
        public DateTime? Birthday { get; set; } // Ngày sinh

        public float? Weight { get; set; } // Cân nặng

        public bool? Neutered { get; set; } // Đã triệt sản chưa

        public string? OriginalColor { get; set; } // Màu lông ban đầu

        public bool? Vaccinated { get; set; } // Đã tiêm phòng chưa

        public string? Note { get; set; } // Ghi chú
        public string? Address { get; set; }
        public bool IsDelete { get; set; }
    }
}
