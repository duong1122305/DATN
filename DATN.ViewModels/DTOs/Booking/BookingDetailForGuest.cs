using DATN.ViewModels.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
    public class BookingDetailForGuest
    {
        public int? IdBooking { get; set; }
        public string PetName { get; set; }
        public string ServiceName { get; set; }
        public int ServiceId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public double TotalPrice { get; set; }
        public BookingDetailStatus Status { get; set; }
    }
}
