using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;

namespace DATN.Aplication.Services.IServices
{
    public interface IBookingManagement
    {
        public Task<ResponseData<List<BookingView>>> GetListBookingInOneWeek();
    }
}