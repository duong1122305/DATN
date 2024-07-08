using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
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

        public Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<List<BookingView>>> GetAll()
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<List<BookingView>>>("/api/Booking/List");
        }

        public async Task<ResponseData<List<ListBokingDetailInDay>>> ListBookingDetailInDay(string id, DateTime date)
        {
            return _httpClient.GetFromJsonAsync<ResponseData<List<ListBokingDetailInDay>>>($"/api/Booking/List-Booking-Detail-In-Day?id={id}&date={date.Year}-{date.Month}-{date.Day}").GetAwaiter().GetResult();
        }

        public async Task<ResponseData<List<NumberOfScheduleView>>> ListStaffFreeInTime(string from, string to)
        {
            return _httpClient.GetFromJsonAsync<ResponseData<List<NumberOfScheduleView>>>($"/api/Booking/List-Staff-Free-In-Time?from={from}&to={to}").GetAwaiter().GetResult();
        }
    }
}
