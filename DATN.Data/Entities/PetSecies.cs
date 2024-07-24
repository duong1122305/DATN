using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Data.Entities
{
    // Bảng Giống thú cưng
    [Table("PetSpecies")]
    public class PetSpecies
    {
        public int Id { get; set; } // Khóa chính

        public int PetTypeId { get; set; } // Khóa ngoại đến ID loại thú cưng

        public string Name { get; set; } // Tên giống thú cưng
        public bool? IsDelete { get; set; }// Xoá mềm loài
        public virtual PetType PetType { get; set; } // Loại thú cưng
        public virtual ICollection<Pet> Pets { get; set; } // Loại thú cưng
    }
}
