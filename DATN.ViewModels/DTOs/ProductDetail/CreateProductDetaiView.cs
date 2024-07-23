using System.ComponentModel.DataAnnotations;

namespace DATN.ViewModels.DTOs.ProductDetail
{
    public class CreateProductDetaiView
    {
        public int Id { get; set; }
        public int IdProduct { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1000, double.MaxValue)]
        public double Price { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Amount { get; set; }

    }
}
