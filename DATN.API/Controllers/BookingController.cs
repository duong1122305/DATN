using DATN.Aplication.Services;
using DATN.Aplication.Services.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        private readonly IBookingManagement _bookingManagement;

        public BookingController(IBookingManagement bookingManagement)
        {
            _bookingManagement = bookingManagement;
        }
        [HttpGet("List")]
        public Task<ResponseData<List<BookingView>>> Index()
        {
            return _bookingManagement.GetListBookingInOneWeek();
        }
    }
}
