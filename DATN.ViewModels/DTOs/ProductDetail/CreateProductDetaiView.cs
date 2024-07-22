using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.ProductDetail
{
    public class CreateProductDetaiView
    {
        public int Id { get; set; }
        public int IdProduct { get; set; }
        [Required(ErrorMessage = "Không tên biến thể")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Không để trống giá")]
        [Range(1000,double.MaxValue, ErrorMessage = "Giá trên 1000 vnđ")]
        public double Price { get; set; }
        [Required]
        [Range(0,int.MaxValue,ErrorMessage ="Số lượng từ 0 trở lên")]
        public int Amount { get; set; }
      
    }
}
