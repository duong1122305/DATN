using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace DATN.ViewModels.DTOs.Guest
{
    public class GuestRegisterUserRequest
    {
        public string Name { get; set; } // Tên khách hàng
        public bool? Gender { get; set; } // Giới tính
        [RegularExpression("/(84|0[3|5|7|8|9])+([0-9]{8})\b/g", ErrorMessage ="Sai định dạng số điện thoại")]
        public string PhoneNumber { get; set; } // Số điện thoại

    
        public string Password { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string? Address { get; set; }
       
       
    }
}
