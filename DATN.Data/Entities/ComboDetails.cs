using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Data.Entities
{
    // Bảng Chi tiết gói dịch vụ
    [Table("ComboDetails")]
    public class ComboDetail
    {
        public int ServiceDetailId { get; set; } // Khóa ngoại đến ID dịch vụ

        public int ComboServiceId { get; set; } // Khóa ngoại đến ID gói dịch vụ

        public virtual ServiceDetail ServiceDetail { get; set; } // Dịch vụ

        public virtual ComboService ComboService { get; set; } // Gói dịch vụ
    }
}
