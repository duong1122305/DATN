using DATN.Aplication.Services.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ActionBooking;
using DATN.ViewModels.DTOs.Authenticate;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.DTOs.Payment;
using DATN.ViewModels.DTOs.Product;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        private readonly IBookingManagement _bookingManagement;
        private readonly IEmployeeScheduleManagementService _employeeScheduleManagementService;
        private readonly IProductManagement _productManagement;
        private readonly IVoucherManagementService _voucherManagementService;
        public BookingController(IBookingManagement bookingManagement, IEmployeeScheduleManagementService employeeScheduleManagementService, IProductManagement productManagement, IVoucherManagementService voucherManagementService)
        {
            _bookingManagement = bookingManagement;
            _employeeScheduleManagementService = employeeScheduleManagementService;
            _productManagement = productManagement;
            _voucherManagementService = voucherManagementService;
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
        public async Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest, string token)
        {
            return await _bookingManagement.CreateBookingInStore(createBookingRequest, token);
        }
        [HttpGet("List-Staff-Free-In-Time")]
        public async Task<ResponseData<List<NumberOfScheduleView>>> ListStaffFreeInTime(string from, string to, DateTime dateTime)
        {
            return await _employeeScheduleManagementService.ListStaffFreeInTime(TimeSpan.Parse(from), TimeSpan.Parse(to), dateTime);
        }
        [HttpGet("Get-Bill-Of-Guest")]
        public async Task<ResponseData<Bill>> GetBillOfGuest(Guid idguest, DateTime dateBooking)
        {
            return await _bookingManagement.GetBill(idguest, dateBooking);
        }

        [HttpPut("Complete-Booking")]
        public async Task<ResponseData<string>> CompleteBooking(ActionView actionView)
        {
            return await _bookingManagement.CompleteBooking(actionView);
        }
        [HttpPut("start-booking")]
        public async Task<ResponseData<string>> StartBooking(ActionView actionView)
        {
            return await _bookingManagement.StartBooking(actionView);
        }
        [HttpPut("canel-booking")]
        public async Task<ResponseData<string>> CancelBooking(ActionView actionView)
        {
            return await _bookingManagement.CancelBooking(actionView);
        }
        [HttpPut("start-booking-details")]
        public async Task<ResponseData<string>> StartBookingDetail(ActionView actionView)
        {
            return await _bookingManagement.StartBookingDetail(actionView);
        }
        [HttpPut("cancel-booking-details")]
        public async Task<ResponseData<string>> CancelBookingDetail(ActionView actionView)
        {
            return await _bookingManagement.CancelBookingDetail(actionView);
        }
        [HttpPut("complete-bookingDetails")]
        public async Task<ResponseData<string>> CompleteBookingDetail(ActionView actionView)
        {
            return await _bookingManagement.CompleteBookingDetail(actionView);
        }
        [HttpPut("Confirm-booking")]
        public async Task<ResponseData<string>> ConfirmBooking(ActionView actionView)
        {
            return await _bookingManagement.ConfirmBooking(actionView);
        }
        [HttpPost("Guest-Booking")]
        public async Task<ResponseData<string>> GuestBooking(CreateBookingRequest createBookingRequest)
        {
            return await _bookingManagement.GuestCreateBooking(createBookingRequest);
        }

        [HttpPost("Add-Product-For-Bill")]
        public async Task<ResponseData<string>> BuyProduct(List<BuyProduct> buyProducts)
        {
            return await _productManagement.BuyProduct(buyProducts);
        }
        [HttpGet("List-Product-View-Sale")]
        public async Task<ResponseData<List<ProductSelect>>> ListProductViewSale()
        {
            return await _productManagement.ListProductViewSale();
        }
        [HttpPost("Check-bill")]
        public async Task<ResponseData<Bill>> CheckBill(int? idBooking, List<ProductDetailView> productdes)
        {
            return await _bookingManagement.CheckBill(idBooking ?? null, productdes);
        }
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
        [HttpGet("QrCode-CheckIn")]
        public async Task<ResponseData<string>> QrCodeCheckIn(int idBooking)
        {
            return await _bookingManagement.QrCodeCheckIn(idBooking);
        }
        [HttpGet("Payment-Qr")]
        public async Task<ResponseData<ResponseMomo>> PaymentQr(string totalPrice)
        {
            return await _bookingManagement.PaymentQr(totalPrice);
        }
        [HttpGet("List-Voucher-Can-Apply")]
        public async Task<ResponseData<List<VoucherView>>> ListVoucherCanApply(double totalPrice)
        {
            return await _voucherManagementService.GetAllVoucherCanApply(totalPrice);
        }
    }
}
