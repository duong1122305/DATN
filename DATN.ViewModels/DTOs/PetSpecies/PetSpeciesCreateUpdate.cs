using System.ComponentModel.DataAnnotations;

namespace DATN.ViewModels.DTOs.PetSpecies
{
    public class PetSpeciesCreateUpdate
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Phải chọn loài động vật")]
        public int PetTypeId { get; set; } // Khóa ngoại đến ID loại thú cưng
        [Required(ErrorMessage = "Phải chọn tên loài")]
        [StringLength(50, ErrorMessage = "Tên thú cưng phải có độ dài từ 2-50 ký tư", MinimumLength = 2)]
        public string Name { get; set; } // Tên giống thú cưng
        public bool? IsDelete { get; set; } = false;// Xoá mềm loài
    }
}
