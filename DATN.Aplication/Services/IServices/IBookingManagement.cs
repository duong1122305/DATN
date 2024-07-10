using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;

namespace DATN.Aplication.Services.IServices
{
    public interface IBookingManagement
    {
        public Task<ResponseData<List<BookingView>>> GetListBookingInOneWeek();
        public Task<ResponseData<List<ListBokingDetailInDay>>> GetListBookingDetailInDay(string idGuest, DateTime date);
        public Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest, string token);
        public Task<ResponseData<Bill>> GetBill(Guid IdGuest, DateTime dateBooking);
        public Task<ResponseData<string>> CompleteBooking(int id);
        public Task<ResponseData<string>> StartBooking(int id);
        public Task<ResponseData<string>> CancelBooking(int id, string reason, string token);
        public Task<ResponseData<string>> StartBookingDetail(int id);
        public Task<ResponseData<string>> CancelBookingDetail(int id, string reason, string token);
        public Task<ResponseData<string>> CompleteBookingDetail(int id);
    }
}