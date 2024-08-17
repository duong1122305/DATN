using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Data.Entities
{
    // Bảng Dịch vụ
    [Table("Services")]
    public class Service
    {
        public int Id { get; set; } // Khóa chính

        public string Name { get; set; } // Tên dịch vụ
        public string? Desciption { get; set; }
        public bool IsDetele { get; set; } = false;

        // Quan hệ một-nhiều: Mỗi dịch vụ có thể được bán trong nhiều gói dịch vụ (combo)
        public virtual ICollection<ServiceDetail> ServiceDetails { get; set; }
    }
}
