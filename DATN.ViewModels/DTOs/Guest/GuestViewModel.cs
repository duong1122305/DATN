using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Guest
{
    public class GuestViewModel
    {
        public Guid Id { get; set; } // Khóa chính
        [Required(ErrorMessage ="Tên khách hàng phải có")]
        public string Name { get; set; } // Tên khách hàng
        public bool? Gender { get; set; } // Giới tính
        public string PhoneNumber { get; set; } // Số điện thoại
        public string? Password { get; set; }
        public string? UserName { get; set; }
        public int? CountPet { get; set; } = 0;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public bool? IsDelete { get; set; }
    }
}
