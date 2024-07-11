using DATN.ADMIN.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using DATN.ViewModels.DTOs.Booking;
using Newtonsoft.Json;
using Syncfusion.Blazor.Gantt.Internal;
using System.Collections.Generic;
using System.Threading.Channels;
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

        public async Task<ResponseData<string>> CancelBooking(int id, string reason, string token)
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<string>>($"/api/Booking/canel-booking?id={id}&reason={reason}&token={token}");

        }

        public async Task<ResponseData<string>> CancelBookingDetail(int id, string reason, string token)
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<string>>($"/api/Booking/cancel-booking-details?id={id}&reason={reason}&token={token}");
        }

        public async Task<ResponseData<string>> CompleteBooking(int id)
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<string>>($"/api/Booking/Complete-Booking?id={id}");
        }

        public async Task<ResponseData<string>> CompleteBookingDetail(int id)
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<string>>($"/api/Booking/complete-bookingDetails?id={id}");
        }

        public async Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest,string token)
            {
            var lst = await _httpClient.PostAsJsonAsync<CreateBookingRequest>($"/api/Booking/Create-Booking?token={token}", createBookingRequest);
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

        public async Task<ResponseData<string>> StartBooking(int id)
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<string>>($"/api/Booking/start-booking?id={id}");
        }

        public async Task<ResponseData<string>> StartBookingDetail(int id)
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<string>>($"/api/Booking/start-booking-details?id={id}");

        }
    }
}
