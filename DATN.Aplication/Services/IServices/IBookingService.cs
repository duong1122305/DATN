using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;

namespace DATN.Aplication.Services.IServices
{
    public interface IBookingService
    {
        Task<ResponseData<string>> CreateBooking(CreateBookingRequest request);
    }
}
