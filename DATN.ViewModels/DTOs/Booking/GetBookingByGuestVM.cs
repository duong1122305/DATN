using DATN.Data.Enum;
using DATN.ViewModels.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
    public class GetBookingByGuestVM
    {
        public int IdBooking { get; set; }
        public string BookingTime { get; set; }
        public List<BookingDetailForGuest> LstBookingDetail { get; set; }
    }
}
