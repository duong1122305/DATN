using DATN.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
    public class BookingView
    {
        public Guid? IdGuest { get; set; }
        public string NameGuest { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string Address { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime BookingTime { get; set; }
    }
}
