using DATN.ViewModels.DTOs.Product;

namespace DATN.ViewModels.DTOs.Booking
{
    public class Payment
    {
        public Guid? IdGuest { get; set; }
        public int IdBooking { get; set; }
        public string Token { get; set; }
        public DateTime DateBooking { get; set; }
        public int TypePaymenId { get; set; }
        public int? VoucherId { get; set; }
        public double TotalPrice { get; set; }
        public double? Reduce { get; set; }
        public List<ProductDetailView> LstProducts { get; set; }

    }
}
