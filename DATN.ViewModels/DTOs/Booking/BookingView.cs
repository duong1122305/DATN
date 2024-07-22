using DATN.Data.Enum;

namespace DATN.ViewModels.DTOs.Booking
{
    public class BookingView
    {
        public Guid? IdGuest { get; set; }
        public int? IdBooking { get; set; }
        public string NameGuest { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string Address { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime BookingTime { get; set; }
        public bool IsPayment { get; set; }
    }
}
