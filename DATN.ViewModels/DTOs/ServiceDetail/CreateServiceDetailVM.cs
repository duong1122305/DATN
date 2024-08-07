﻿using System.ComponentModel.DataAnnotations;

namespace DATN.ViewModels.DTOs.ServiceDetail
{
    public class CreateServiceDetailVM
    {
        [Required(ErrorMessage = "Chưa chọn dịch vụ")]
        public int ServiceId { get; set; } // Khóa ngoại đến ID dịch vụ

        //[Required(ErrorMessage = "Bắt buộc nhập tên dịch vụ chi tiết")]
        //[MaxLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        //[MinLength(5, ErrorMessage = "Tối thiểu 5 ký tự")]
        //public string Name { get; set; } // Tên chi tiết dịch vụ

        [Range(1000, 9999999, ErrorMessage = "Giá không được nhỏ hơn 100 hoặc lớn hơn 9999999")]
        public float Price { get; set; } // Giá tiền

        [Range(1, 9999, ErrorMessage = "Thời gian phải lớn hơn 1 và ít hơn 9999")]
        public double Duration { get; set; } // Khoảng thời gian

        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string Description { get; set; } // Mô tả

        [Required(ErrorMessage = "Bắt buộc nhập cân nặng tối thiểu")]
        [Range(1, 100, ErrorMessage = "Cân nặng không được nhỏ hơn 1 hoặc lớn hơn 100")]
        public float MinWeight { get; set; }

        [Required(ErrorMessage = "Bắt buộc nhập cân nặng tối đa")]
        [Range(1, 100, ErrorMessage = "Cân nặng không được nhỏ hơn 1 hoặc lớn hơn 100")]
        public float MaxWeight { get; set; }

        public bool IsDeleted { get; set; } = false; // Mô tả

        public DateTime CreateAt { get; set; } // Thời gian tạo
    }
}
