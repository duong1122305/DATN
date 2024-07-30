using System.ComponentModel.DataAnnotations;

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
        [Range(1,int.MaxValue)]
        public int Amount { get; set; }

    }
}
