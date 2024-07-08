using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;

namespace DATN.Aplication.Services.IServices
{
    public interface IBookingManagement
    {
        public Task<ResponseData<List<BookingView>>> GetListBookingInOneWeek();
        public Task<ResponseData<List<ListBokingDetailInDay>>> GetListBookingDetailInDay(string idGuest, DateTime date);
        public Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest);
        public Task<ResponseData<Bill>> GetBill(Guid IdGuest, DateTime dateBooking);
    }
}