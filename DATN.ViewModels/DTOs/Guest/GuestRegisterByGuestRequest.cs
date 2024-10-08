﻿using DATN.Utilites.Validator;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace DATN.ViewModels.DTOs.Guest
{
    public class GuestRegisterByGuestRequest
    {
        [Required(ErrorMessage = "Tên khách hàng bắt buộc phải nhập")]
        public string Name { get; set; } // Tên khách hàng
        public bool? Gender { get; set; } // Giới tính
        [PasswordValidation]
        public string Password { get; set; }
        [RegularExpression(@"^(09|03|07|08|05)\d{8}$", ErrorMessage = "Nhập đúng định dạng số điện thoại Việt Nam")]
        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        public string PhoneNumber { get; set; } // Số điện thoại
        public string? Address { get; set; }
        [EmailAddress(ErrorMessage = "Email sai định dạng")]
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string UserName { get; set; }
        public IFormFile? AvatarFile { get; set; }
    }
}
