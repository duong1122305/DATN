using DATN.Data.Enum;

namespace DATN.ViewModels.DTOs.Booking
{
    public class CreateBookingRequest
    {
        public List<CreateBookingDetailRequest> ListIdServiceDetail { get; set; }
        public int? VoucherId { get; set; }
        public int IdBooking { get; set; }
        public double? ReducedAmount { get; set; }
        public int? PaymentTypeId { get; set; }
        public Guid GuestId { get; set; }
        public string? GuestName { get; set; }
        public double? TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
    }
}
