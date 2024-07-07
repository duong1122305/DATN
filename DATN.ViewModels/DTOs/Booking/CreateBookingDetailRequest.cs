using DATN.ViewModels.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
    public class CreateBookingDetailRequest
    {
        public int BookingId { get; set; } 

        public Guid? StaffId { get; set; }

        public int PetId { get; set; }

        public int? ComboId { get; set; }

        public int ServiceDetailId { get; set; } 
        public DateTime StartDateTime { get; set; } 

        public DateTime EndDateTime { get; set; }

        public BookingDetailStatus Status { get; set; } 

        public double Price { get; set; }
    }
}
