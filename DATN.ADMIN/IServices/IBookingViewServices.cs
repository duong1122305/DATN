using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ActionBooking;
using DATN.ViewModels.DTOs.Authenticate;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.DTOs.Payment;
using DATN.ViewModels.DTOs.Product;

namespace DATN.ADMIN.IServices
{
    public interface IBookingViewServices
    {
        Task<ResponseData<List<BookingView>>> GetAll();
        Task<ResponseData<List<ListBokingDetailInDay>>> ListBookingDetailInDay(int id);
        Task<ResponseData<List<NumberOfScheduleView>>> ListStaffFreeInTime(string from, string to, DateTime dateTime);
        Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest, string token);
        Task<ResponseData<Bill>> GetBillOfGuest(Guid idguest, DateTime dateBooking);
        public Task<ResponseData<string>> CompleteBooking(ActionView actionView);
        public Task<ResponseData<string>> ConfirmBooking(ActionView actionView);
        public Task<ResponseData<string>> StartBooking(ActionView actionView);
        public Task<ResponseData<string>> CancelBooking(ActionView actionView);
        public Task<ResponseData<string>> StartBookingDetail(ActionView actionView);
        public Task<ResponseData<string>> CancelBookingDetail(ActionView actionView);
        public Task<ResponseData<string>> CompleteBookingDetail(ActionView actionView);
        public Task<ResponseData<string>> BuyProduct(List<BuyProduct> buyProducts);
        public Task<ResponseData<List<ProductSelect>>> ListProductViewSale();
        public Task<ResponseData<Bill>> CheckBill(int? idBooking, List<ProductDetailView> productdes);
        public Task<ResponseData<string>> PaymentInStore(Payment payment);
        public Task<ResponseData<string>> QrCodeCheckIn(int idBooking);
        public Task<ResponseData<string>> QrCodeCheckOut(int idBookingDetail);
        public Task<ResponseData<ResponseMomo>> PaymentQr(Payment payment, int? id);
        public Task<ResponseData<string>> PaymentQrVnPay(long totalPrice);
        public Task<ResponseData<string>> CheckInArrive(int idBooking);
        public Task<ResponseData<string>> AddService(CreateServiceDetail createBookingDetailRequest);
    }
}
