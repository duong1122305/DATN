using DATN.ViewModels.DTOs.Authenticate;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;

namespace DATN.ADMIN.IServices
{
    public interface IBookingViewServices
    {
        Task<ResponseData<List<BookingView>>> GetAll();
        Task<ResponseData<List<ListBokingDetailInDay>>> ListBookingDetailInDay(string id, DateTime date);
        Task<ResponseData<List<NumberOfScheduleView>>> ListStaffFreeInTime(string from, string to);
        Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest, string token);
        Task<ResponseData<Bill>> GetBillOfGuest(Guid idguest, DateTime dateBooking);
        Task<ResponseData<string>> CompleteBooking(int id);
        Task<ResponseData<string>> StartBooking(int id);
        Task<ResponseData<string>> CancelBooking(int id, string reason, string token);
        Task<ResponseData<string>> StartBookingDetail(int id);
        Task<ResponseData<string>> CancelBookingDetail(int id, string reason, string token);
        Task<ResponseData<string>> CompleteBookingDetail(int id);
    }
}
