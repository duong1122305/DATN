using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Data.Entities
{
    // Bảng Khách không có tài khoản
    [Table("Guests")]
    public class Guest
    {
        public Guid Id { get; set; } // Khóa chính
        public string Name { get; set; } // Tên khách hàng
        public bool? Gender { get; set; } // Giới tính
        public string PhoneNumber { get; set; } // Số điện thoại
        public string? PasswordHash { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? CodeConfirm { get; set; }
        public bool? IsComfirm { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public string? VerifyCode { get; set; }
        public DateTime? RegisteredAt { get; set; } // Thời gian đăng ký
        public bool? IsDeleted { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }
    }
}
