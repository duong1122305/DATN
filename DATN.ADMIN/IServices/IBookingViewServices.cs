using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;

namespace DATN.ADMIN.IServices
{
    public interface IBookingViewServices
    {
        Task<ResponseData<List<BookingView>>> GetAll();
        Task<ResponseData<List<ListBokingDetailInDay>>> ListBookingDetailInDay(string id, DateTime date);
    }
}
