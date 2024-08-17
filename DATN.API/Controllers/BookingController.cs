using DATN.API.Services;
using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ActionBooking;
using DATN.ViewModels.DTOs.Authenticate;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.DTOs.Payment;
using DATN.ViewModels.DTOs.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DATN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class BookingController : Controller
    {
        private readonly IBookingManagement _bookingManagement;
        private readonly IEmployeeScheduleManagementService _employeeScheduleManagementService;
        private readonly IProductManagement _productManagement;
        private readonly IVoucherManagementService _voucherManagementService;
        private readonly IHubContext<BookingHub> _hubContext;
        public BookingController(IBookingManagement bookingManagement, IEmployeeScheduleManagementService employeeScheduleManagementService, IProductManagement productManagement, IVoucherManagementService voucherManagementService, IHubContext<BookingHub> hubContext)
        {
            _bookingManagement = bookingManagement;
            _employeeScheduleManagementService = employeeScheduleManagementService;
            _productManagement = productManagement;
            _voucherManagementService = voucherManagementService;
            _hubContext = hubContext;
        }
        [HttpGet("List")]
        [Authorize(Roles = "Admin,Receptionist")]
        public Task<ResponseData<List<BookingView>>> Index()
        {
            return _bookingManagement.GetListBookingInOneWeek();
        }
        [Authorize(Roles = "Admin,Receptionist")]
        [HttpGet("List-Booking-Detail-In-Day")]
        public Task<ResponseData<List<ListBokingDetailInDay>>> ListBookingDetailInDay(int id)
        {
            return _bookingManagement.GetListBookingDetailInDay(id);
        }

        [HttpPost("Create-Booking")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest, string token)
        {
            var result = await _bookingManagement.CreateBookingInStore(createBookingRequest, token);
            return result;
        }

        [HttpGet("List-Staff-Free-In-Time")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<ResponseData<List<NumberOfScheduleView>>> ListStaffFreeInTime(string from, string to, DateTime dateTime)
        {
            return await _employeeScheduleManagementService.ListStaffFreeInTime(TimeSpan.Parse(from), TimeSpan.Parse(to), dateTime);
        }

        [HttpGet("Get-Bill-Of-Guest")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<ResponseData<Bill>> GetBillOfGuest(Guid idguest, DateTime dateBooking)
        {
            return await _bookingManagement.GetBill(idguest, dateBooking);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPatch("Complete-Booking")]
        public async Task<ResponseData<string>> CompleteBooking(ActionView actionView)
        {
            return await _bookingManagement.CompleteBooking(actionView);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPatch("start-booking")]
        public async Task<ResponseData<string>> StartBooking(ActionView actionView)
        {
            return await _bookingManagement.StartBooking(actionView);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPatch("canel-booking")]
        public async Task<ResponseData<string>> CancelBooking(ActionView actionView)
        {
            return await _bookingManagement.CancelBooking(actionView);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPatch("start-booking-details")]
        public async Task<ResponseData<string>> StartBookingDetail(ActionView actionView)
        {
            return await _bookingManagement.StartBookingDetail(actionView);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPatch("cancel-booking-details")]
        public async Task<ResponseData<string>> CancelBookingDetail(ActionView actionView)
        {
            return await _bookingManagement.CancelBookingDetail(actionView);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPatch("complete-bookingDetails")]
        public async Task<ResponseData<string>> CompleteBookingDetail(ActionView actionView)
        {
            return await _bookingManagement.CompleteBookingDetail(actionView);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPatch("Confirm-booking")]
        public async Task<ResponseData<string>> ConfirmBooking(ActionView actionView)
        {
            return await _bookingManagement.ConfirmBooking(actionView);
        }

        [HttpPost("Guest-Booking")]
        public async Task<ResponseData<string>> GuestBooking(CreateBookingRequest createBookingRequest)
        {
            var result = await _bookingManagement.GuestCreateBooking(createBookingRequest);
            if (result.IsSuccess)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveBookingNotification", $"Booking đã được tạo thành công bởi ID: {createBookingRequest.GuestName}!");
            }
            return result;
        }
        [Authorize(Roles = "Admin,Receptionist")]
        [HttpGet("List-Product-View-Sale")]
        public async Task<ResponseData<List<ProductSelect>>> ListProductViewSale()
        {
            return await _productManagement.ListProductViewSale();
        }
        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPost("Check-bill")]
        public async Task<ResponseData<Bill>> CheckBill(int? idBooking, List<ProductDetailView> productdes)
        {
            return await _bookingManagement.CheckBill(idBooking ?? null, productdes);
        }
        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPost("Payment-In-Store")]
        public async Task<ResponseData<string>> PaymentInStore(Payment payment)
        {
            return await _bookingManagement.PaymentInStore(payment);
        }

        [HttpGet("QrCode-CheckOut")]
        public async Task<ResponseData<string>> QrCodeCheckOut(int idBookingDetail)
        {
            return await _bookingManagement.QrCodeCheckOut(idBookingDetail);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpGet("QrCode-CheckIn")]
        public async Task<ResponseData<string>> QrCodeCheckIn(int idBooking)
        {
            return await _bookingManagement.QrCodeCheckIn(idBooking);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPost("Payment-Qr")]
        public async Task<ResponseData<ResponseMomo>> PaymentQrMomo(int? id, Payment payment)
        {
            return await _bookingManagement.PaymentQrMomo(id, payment);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpGet("Payment-Qr-VnPay")]
        public async Task<ResponseData<string>> PaymentQrVnPay(long totalPrice)
        {
            return await _bookingManagement.PaymentQrVnPay(totalPrice);
        }

        [HttpGet("List-Voucher-Can-Apply")]
        public async Task<ResponseData<List<VoucherView>>> ListVoucherCanApply(double totalPrice)
        {
            return await _voucherManagementService.GetAllVoucherCanApply(totalPrice);
        }

        [HttpGet("CheckIn-Booking")]
        public async Task<ResponseData<string>> CheckInArrive(int idBooking)
        {
            return await _bookingManagement.CheckInArrive(idBooking);
        }

        [HttpGet("GetBookingByGuest")]
        public async Task<ResponseData<List<GetBookingByGuestVM>>> GetBookingByGuest(Guid idGuest)
        {
            return await _bookingManagement.GetBookingByGuest(idGuest);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPost("Add-Service-For-Booking")]
        public async Task<ResponseData<string>> AddService(AddBookingDetail createBookingDetailRequest)
        {
            return await _bookingManagement.AddService(createBookingDetailRequest);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPost("Check-Status/{id}")]
        public async Task<IActionResult> Check([FromBody] MomoResultRequest momoResultRequest, int id)
        {
            await _bookingManagement.CheckStatusPayment(id, momoResultRequest);
            return StatusCode(204);
        }

        [HttpPost("Create-Booking-For-User-NoAccount")]
        public async Task<ResponseData<string>> CreateBookingForUserNoAccount(BookingForGuestNoAccount booking)
        {
            return await _bookingManagement.CreateBookingForGuestNoAcount(booking);
        }

        [HttpPatch("Cancel-BookingDetail-ByGuest")]
        public async Task<ResponseData<string>> CancelBookingDetailByGuest(ActionView actionView)
        {
            return await _bookingManagement.CancelBookingDetailByGuest(actionView);
        }
    }
}
