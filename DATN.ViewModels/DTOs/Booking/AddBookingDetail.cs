using DATN.ViewModels.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
    public class AddBookingDetail
    {
        public int BookingId { get; set; }
        public string Token { get; set; }
        public int? PetId { get; set; }
        public int? ComboId { get; set; }
        public List<BookingDetailView> ListServiceDetail { get; set; }
    }
}
