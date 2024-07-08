using DATN.Aplication.Services;
using DATN.Aplication.Services.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using DATN.ViewModels.DTOs.Booking;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        private readonly IBookingManagement _bookingManagement;
        private readonly IEmployeeScheduleManagementService _employeeScheduleManagementService;

        public BookingController(IBookingManagement bookingManagement,IEmployeeScheduleManagementService employeeScheduleManagementService)
        {
            _bookingManagement = bookingManagement;
            _employeeScheduleManagementService = employeeScheduleManagementService;
        }
        [HttpGet("List")]
        public Task<ResponseData<List<BookingView>>> Index()
        {
            return _bookingManagement.GetListBookingInOneWeek();
        }
        [HttpGet("List-Booking-Detail-In-Day")]
        public Task<ResponseData<List<ListBokingDetailInDay>>> ListBookingDetailInDay(string id, DateTime date)
        {
            return _bookingManagement.GetListBookingDetailInDay(id, date);
        }
        [HttpPost("Create-Booking")]
        public async Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest)
        {
            return await _bookingManagement.CreateBookingStore(createBookingRequest);
        }
        [HttpGet("List-Staff-Free-In-Time")]
        public async Task<ResponseData<List<NumberOfScheduleView>>> ListStaffFreeInTime(TimeSpan from, TimeSpan to)
        {
            return await _employeeScheduleManagementService.ListStaffFreeInTime(from, to);
        }
    }
}
