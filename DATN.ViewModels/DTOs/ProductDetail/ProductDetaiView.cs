using DATN.ViewModels.Enum;

namespace DATN.ViewModels.DTOs.ProductDetail
{
    public class ProductDetaiView
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public ProductDetailStatus Status { get; set; }
    }
}
