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
        public async Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest,string token)
        {
            return await _bookingManagement.CreateBookingStore(createBookingRequest,token);
        }
        [HttpGet("List-Staff-Free-In-Time")]
        public async Task<ResponseData<List<NumberOfScheduleView>>> ListStaffFreeInTime(string from, string to)
        {
            return await _employeeScheduleManagementService.ListStaffFreeInTime(TimeSpan.Parse(from), TimeSpan.Parse(to));
        }
        [HttpGet("Get-Bill-Of-Guest")]
        public async Task<ResponseData<Bill>> GetBillOfGuest(Guid idguest,DateTime dateBooking)
        {
            return await _bookingManagement.GetBill(idguest, dateBooking);
        }

        [HttpPost("Complete-Booking")]
        public async Task<ResponseData<string>> CompleteBooking(int id)
        {
            return await _bookingManagement.CompleteBooking(id);
        }
        [HttpPost("start-booking")]
        public async Task<ResponseData<string>> StartBooking(int id)
        {
            return await _bookingManagement.StartBooking(id);
        }
        [HttpPost("canel-booking")]
        public async Task<ResponseData<string>> CancelBooking(int id, string reason, string token)
        {
            return await _bookingManagement.CancelBooking(id, reason, token);
        }
        [HttpPost("start-booking-details")]
        public async Task<ResponseData<string>> StartBookingDetail(int id)
        {
            return await _bookingManagement.StartBookingDetail(id);
        }
        [HttpPost("cancel-booking-details")]
        public async Task<ResponseData<string>> CancelBookingDetail(int id, string reason, string token)
        {
            return await _bookingManagement.CancelBookingDetail(id, reason, token);
        }
        [HttpPost("complete-bookingDetails")]
        public async Task<ResponseData<string>> CompleteBookingDetail(int id)
        {
            return await _bookingManagement.CompleteBookingDetail(id);
        }
    }
}
