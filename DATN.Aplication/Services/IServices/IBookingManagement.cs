using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ActionBooking;
using DATN.ViewModels.DTOs.Booking;

namespace DATN.Aplication.Services.IServices
{
    public interface IBookingManagement
    {
        public Task<ResponseData<List<BookingView>>> GetListBookingInOneWeek();
        public Task<ResponseData<List<ListBokingDetailInDay>>> GetListBookingDetailInDay(string idGuest, DateTime date);
        public Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest, string token);
        public Task<ResponseData<Bill>> GetBill(Guid IdGuest, DateTime dateBooking);
        public Task<ResponseData<string>> CompleteBooking(ActionView actionView);
        public Task<ResponseData<string>> StartBooking(ActionView actionView);
        public Task<ResponseData<string>> CancelBooking(ActionView actionView);
        public Task<ResponseData<string>> StartBookingDetail(ActionView actionView);
        public Task<ResponseData<string>> CancelBookingDetail(ActionView actionView);
        public Task<ResponseData<string>> CompleteBookingDetail(ActionView actionView);
        public Task<ResponseData<string>> ConfirmBooking(ActionView actionView);
    }
}