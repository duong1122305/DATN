using DATN.ViewModels.Enum;

namespace DATN.ViewModels.DTOs.Product
{
    public class ProductDetailView
    {
        public int IdProductDetail { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int SelectQuantityProduct { get; set; } = 1;
        public int IdBooking { get; set; }
        public ProductDetailStatus Status { get; set; }
        public int? Term { get; set; }
    }
}
