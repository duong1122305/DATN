using DATN.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
    public class CreateBookingRequest
    {
        public List<CreateBookingDetailRequest> ListIdServiceDetail { get; set; }
        public int? VoucherId { get; set; } 
        public double? ReducedAmount { get; set; }
        public int? PaymentTypeId { get; set; }
        public Guid GuestId { get; set; }
        public double TotalPrice { get; set; }
        public BookingStatus Status { get; set; } 
    }
}
