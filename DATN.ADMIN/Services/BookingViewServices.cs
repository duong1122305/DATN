using Azure;
using DATN.ADMIN.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ActionBooking;
using DATN.ViewModels.DTOs.Authenticate;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.DTOs.Payment;
using DATN.ViewModels.DTOs.Product;
using Newtonsoft.Json;
using Syncfusion.Blazor.Gantt.Internal;
using Syncfusion.Blazor.Schedule.Internal;
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

        public async Task<ResponseData<string>> BuyProduct(List<BuyProduct> buyProducts)
        {
            var response = await _httpClient.PostAsJsonAsync<List<BuyProduct>>($"/api/Booking/Add-Product-For-Bill", buyProducts);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<string>> CancelBooking(ActionView actionView)
        {
            var response = await _httpClient.PutAsJsonAsync<ActionView>($"/api/Booking/canel-booking", actionView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<string>> CancelBookingDetail(ActionView actionView)
        {
            var response = await _httpClient.PutAsJsonAsync<ActionView>($"/api/Booking/cancel-booking-details", actionView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<Bill>> CheckBill(int? idBooking, List<ProductDetailView> productdes)
        {

            var response = await _httpClient.PostAsJsonAsync<List<ProductDetailView>>($"/api/Booking/Check-bill?idBooking={idBooking}", productdes);
            var result = JsonConvert.DeserializeObject<ResponseData<Bill>>(await response.Content.ReadAsStringAsync());
            return result;

        }

        public async Task<ResponseData<string>> CompleteBooking(ActionView actionView)
        {
            var response = await _httpClient.PutAsJsonAsync<ActionView>($"/api/Booking/Complete-Booking", actionView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<string>> CompleteBookingDetail(ActionView actionView)
        {
            var response = await _httpClient.PutAsJsonAsync<ActionView>($"/api/Booking/complete-bookingDetails", actionView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<string>> ConfirmBooking(ActionView actionView)
        {
            var response = await _httpClient.PutAsJsonAsync<ActionView>($"/api/Booking/Confirm-booking", actionView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest, string token)
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

        public async Task<ResponseData<List<ProductSelect>>> ListProductViewSale()
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<List<ProductSelect>>>("/api/Booking/List-Product-View-Sale");
        }

        public async Task<ResponseData<List<NumberOfScheduleView>>> ListStaffFreeInTime(string from, string to, DateTime dateTime)
        {
            return _httpClient.GetFromJsonAsync<ResponseData<List<NumberOfScheduleView>>>($"/api/Booking/List-Staff-Free-In-Time?from={from}&to={to}&datetime={dateTime.Year}-{dateTime.Month}-{dateTime.Day}").GetAwaiter().GetResult();
        }

        public async Task<ResponseData<string>> StartBooking(ActionView actionView)
        {
            var response = await _httpClient.PutAsJsonAsync<ActionView>($"/api/Booking/start-booking", actionView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<string>> StartBookingDetail(ActionView actionView)
        {
            var response = await _httpClient.PutAsJsonAsync<ActionView>($"/api/Booking/start-booking-details", actionView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;

        }
        public async Task<ResponseData<string>> PaymentInStore(Payment payment)
        {
            var response = await _httpClient.PostAsJsonAsync<Payment>($"/api/Booking/Payment-In-Store", payment);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<string>> QrCodeCheckIn(int idBooking)
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<string>>("/api/Booking/QrCode-CheckIn");
        }

        public async Task<ResponseData<string>> QrCodeCheckOut(int idBookingDetail)
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<string>>("/api/Booking/QrCode-CheckOut");

        }

        public async Task<ResponseData<ResponseMomo>> PaymentQr(string totalPrice)
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<ResponseMomo>>($"/api/Booking/Payment-Qr?totalPrice={totalPrice}");
        }
    }
}
