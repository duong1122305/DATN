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
        public List<string> ServiceName { get; set; }
        public List<int> ServiceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalPrice { get; set; }
        public BookingDetailStatus Status { get; set; }
    }
}
