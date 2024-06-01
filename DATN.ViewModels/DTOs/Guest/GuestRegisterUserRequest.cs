using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DATN.Utilites.Validator;
namespace DATN.ViewModels.DTOs.Guest
{
    public class GuestRegisterUserRequest
    {
        [Required(ErrorMessage ="Tên khách hàng bắt buộc phải nhập")]
        public string Name { get; set; } // Tên khách hàng
        public bool? Gender { get; set; } // Giới tính
        [RegularExpression(@"^(09|03|07|08|05)\d{8}$", ErrorMessage = "Nhập đúng định dạng số điện thoại Việt Nam")]
        public string PhoneNumber { get; set; } // Số điện thoại

        [PasswordValidation]
        public string Password { get; set; }
        [MinLength(6, ErrorMessage ="Độ dài tên đăng nhập lớn hơn 4 ký tự")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage ="Phải đúng định dạng email")]
        public string Email { get; set; }
        public string? Address { get; set; }
       
       
    }
}
