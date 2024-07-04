using DATN.Data.Enum;
using DATN.Utilites;
using DATN.ViewModels.DTOs.BookingManager;
using DATN.ViewModels.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
	public class CreateBookingRequest
	{
        public float TotalPrice { get; set; }
        public Guid GuestId { get; set; }= Contant.GuestsID;
        public BookingStatus Status { get; set; }
        [Required(ErrorMessage ="Phải có ít nhất 1 dịch vụ trong hoá đơn")]

        public List<BookingDetailCreateRequest> lstBookingDetail {  get; set; }
    }
}
