using System.ComponentModel.DataAnnotations;

namespace DATN.ViewModels.DTOs.ProductDetail
{
    public class CreateProductDetaiView
    {
        public int Id { get; set; }
        public int IdProduct { get; set; }
        [Required(ErrorMessage = "Phải có tên biến thể")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Phải có giá")]
        [Range(1000.0,100000000.0, ErrorMessage = "Giá từ 1.000vnđ- 100.000.000vnđ")]
        public double Price { get; set; }
        [Required( ErrorMessage = "Phải nhập số lượng")]
        [Range(1,1000000,ErrorMessage ="Số lượng phải từ 1-100.000")]
        public int Amount { get; set; }

    }
}
