using DATN.ADMIN.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using DATN.ViewModels.DTOs.Booking;
using Newtonsoft.Json;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DATN.ADMIN.Services
{
    public class BookingViewServices : IBookingViewServices
    {
        private readonly HttpClient _httpClient;
        public BookingViewServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest)
        {
            var lst = await _httpClient.PostAsJsonAsync<CreateBookingRequest>("/api/Booking/Create-Booking", createBookingRequest);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await lst.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<List<BookingView>>> GetAll()
        {
            return _httpClient.GetFromJsonAsync<ResponseData<List<BookingView>>>("/api/Booking/List").GetAwaiter().GetResult();
        }

        public async Task<ResponseData<Bill>> GetBillOfGuest(Guid idguest, DateTime dateBooking)
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<Bill>>($"/api/Booking/Get-Bill-Of-Guest?idguest={idguest}&dateBooking={dateBooking.Year}-{dateBooking.Month}-{dateBooking.Day}");
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
