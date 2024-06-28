using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.ServiceDetail
{
    public class UpdateServiceDetailVM
    {
        [Range(1000, 9999999, ErrorMessage = "Giá không được nhỏ hơn 100 hoặc lớn hơn 9999999")]
        public float Price { get; set; }

        [Range(1, 9999, ErrorMessage = "Thời gian phải lớn hơn 1 và ít hơn 9999")]
        public double Duration { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Bắt buộc nhập cân nặng tối thiểu")]
        [Range(1, 100, ErrorMessage = "Cân nặng không được nhỏ hơn 1 hoặc lớn hơn 100")]
        public float MinWeight { get; set; }

        [Required(ErrorMessage = "Bắt buộc nhập cân nặng tối đa")]
        [Range(1, 100, ErrorMessage = "Cân nặng không được nhỏ hơn 1 hoặc lớn hơn 100")]
        public float MaxWeight { get; set; }
    }
}
