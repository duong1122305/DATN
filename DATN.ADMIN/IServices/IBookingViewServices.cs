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
        Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest);
    }
}
