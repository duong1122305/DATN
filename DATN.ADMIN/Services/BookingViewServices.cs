using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;
using System.Collections.Generic;

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

        public async Task<ResponseData<List<ListBokingDetailInDay>>> ListBookingDetailInDay(string id, DateTime date)
        {
            return _httpClient.GetFromJsonAsync<ResponseData<List<ListBokingDetailInDay>>>($"/api/Booking/List-Booking-Detail-In-Day?id={id}&date={date}").GetAwaiter().GetResult();
        }
    }
}
