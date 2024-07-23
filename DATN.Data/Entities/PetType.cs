using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Data.Entities
{
    // Bảng Loại thú cưng
    [Table("PetTypes")]
    public class PetType
    {
        public int Id { get; set; } // Khóa chính

        public string Name { get; set; } // Tên loại thú cưng

        // Quan hệ một-nhiều: Mỗi loại thú cưng có thể có nhiều giống thú cưng
        public virtual ICollection<PetSpecies> Species { get; set; }
    }
}
