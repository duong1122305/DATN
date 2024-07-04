using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;

namespace DATN.ADMIN.Services
{
    public class BookingViewServices : IBookingViewServices
    {
        private readonly HttpClient _httpClient;
        public BookingViewServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseData<List<BookingView>>> GetAll()
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<List<BookingView>>>("/api/Booking/List");
        }
    }
}
