using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
    public class BookingForGuestNoAccount
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string NameGuest { get; set; }
        public string NamePet { get; set; }
        public bool GenderPet { get; set; }
        public int SpeciesId { get; set; }
        public int IdBoooking { get; set; }
        public int? VoucherId { get; set; }
        public List<CreateBookingDetailRequest> LstBookingDetail { get; set; }
    }
}
