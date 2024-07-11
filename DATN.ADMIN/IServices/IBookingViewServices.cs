using DATN.ViewModels.DTOs.Authenticate;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.DTOs.ActionBooking;

namespace DATN.ADMIN.IServices
{
    public interface IBookingViewServices
    {
        Task<ResponseData<List<BookingView>>> GetAll();
        Task<ResponseData<List<ListBokingDetailInDay>>> ListBookingDetailInDay(string id, DateTime date);
        Task<ResponseData<List<NumberOfScheduleView>>> ListStaffFreeInTime(string from, string to);
        Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest, string token);
        Task<ResponseData<Bill>> GetBillOfGuest(Guid idguest, DateTime dateBooking);
        public Task<ResponseData<string>> CompleteBooking(ActionView actionView);
        public Task<ResponseData<string>> ConfirmBooking(ActionView actionView);
        public Task<ResponseData<string>> StartBooking(ActionView actionView);
        public Task<ResponseData<string>> CancelBooking(ActionView actionView);
        public Task<ResponseData<string>> StartBookingDetail(ActionView actionView);
        public Task<ResponseData<string>> CancelBookingDetail(ActionView actionView);
        public Task<ResponseData<string>> CompleteBookingDetail(ActionView actionView);
    }
}
