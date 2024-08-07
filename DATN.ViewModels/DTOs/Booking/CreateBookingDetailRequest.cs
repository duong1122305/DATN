using DATN.ViewModels.Enum;

namespace DATN.ViewModels.DTOs.Booking
{
    public class CreateBookingDetailRequest
    {
        public Guid? StaffId { get; set; }
        public string? StaffName { get; set; }
        public string? ServiceDetailName { get; set; }

        public int? PetId { get; set; }

        public int? ComboId { get; set; }

        public int ServiceDetailId { get; set; }
        public TimeSpan StartDateTime { get; set; }

        public TimeSpan EndDateTime { get; set; }

        public DateTime DateBooking { get; set; }

        public BookingDetailStatus Status { get; set; }

        public double? Price { get; set; }
    }
}
