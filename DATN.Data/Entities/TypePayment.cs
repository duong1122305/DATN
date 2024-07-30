using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Data.Entities
{
    // Bảng Loại thanh toán
    [Table("TypePayments")]
    public class TypePayment
    {
        public int Id { get; set; } // Khóa chính

        public string Name { get; set; } // Tên loại thanh toán
        public virtual List<Booking> Bookings { get; set; }
    }
}
