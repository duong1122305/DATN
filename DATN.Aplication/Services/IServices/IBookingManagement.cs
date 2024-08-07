using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ActionBooking;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.DTOs.Payment;
using DATN.ViewModels.DTOs.Product;

namespace DATN.Aplication.Services.IServices
{
    public interface IBookingManagement
    {
        public Task<ResponseData<string>> AddService(CreateServiceDetail createBookingDetailRequest);
        public Task<ResponseData<List<BookingView>>> GetListBookingInOneWeek();
        public Task<ResponseData<List<ListBokingDetailInDay>>> GetListBookingDetailInDay(int id);
        public Task<ResponseData<string>> CreateBookingInStore(CreateBookingRequest createBookingRequest, string token);
        public Task<ResponseData<Bill>> GetBill(Guid IdGuest, DateTime dateBooking);
        public Task<ResponseData<string>> CompleteBooking(ActionView actionView);
        public Task<ResponseData<string>> StartBooking(ActionView actionView);
        public Task<ResponseData<string>> CancelBooking(ActionView actionView);
        public Task<ResponseData<string>> StartBookingDetail(ActionView actionView);
        public Task<ResponseData<string>> CancelBookingDetail(ActionView actionView);
        public Task<ResponseData<string>> CompleteBookingDetail(ActionView actionView);
        public Task<ResponseData<string>> ConfirmBooking(ActionView actionView);
        public Task<ResponseData<string>> GuestCreateBooking(CreateBookingRequest createBookingRequest);
        public Task<ResponseData<string>> PaymentInStore(Payment payment);
        public Task<ResponseData<Bill>> CheckBill(int? idBooking, List<ProductDetailView> productdes);
        public Task<ResponseData<string>> QrCodeCheckIn(int idBooking);
        public Task<ResponseData<string>> QrCodeCheckOut(int idBookingDetail);
        public Task<ResponseData<ResponseMomo>> PaymentQrMomo(string totalPrice);
        public Task<ResponseData<string>> PaymentQrVnPay(long totalPrice);
        public Task<ResponseData<string>> CheckInArrive(int idBooking);
        public Task<ResponseData<List<GetBookingByGuestVM>>> GetBookingByGuest(Guid idGuest);
    }
}