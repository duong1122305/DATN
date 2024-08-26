using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ActionBooking;
using DATN.ViewModels.DTOs.Authenticate;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.DTOs.Payment;
using DATN.ViewModels.DTOs.Product;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DATN.ADMIN.Services
{
    public class BookingViewServices : IBookingViewServices
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BookingViewServices(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Key"));
        }

        public async Task<ResponseData<string>> BuyProduct(List<BuyProduct> buyProducts)
        {

            var response = await _httpClient.PostAsJsonAsync<List<BuyProduct>>($"/api/Booking/Add-Product-For-Bill", buyProducts);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<string>> CancelBooking(ActionView actionView)
        {
            var response = await _httpClient.PatchAsJsonAsync<ActionView>($"/api/Booking/canel-booking", actionView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<string>> CancelBookingDetail(ActionView actionView)
        {
            var response = await _httpClient.PatchAsJsonAsync<ActionView>($"/api/Booking/cancel-booking-details", actionView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<Bill>> CheckBill(int? idBooking, List<ProductDetailView> productdes)
        {

            if (idBooking != null)
            {
                var response = _httpClient.PostAsJsonAsync<List<ProductDetailView>>($"/api/Booking/Check-bill?idBooking={idBooking}", productdes).GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<ResponseData<Bill>>(await response.Content.ReadAsStringAsync());
                return result;
            }
            else
            {
                var response = _httpClient.PostAsJsonAsync<List<ProductDetailView>>($"/api/Booking/Check-bill?idBooking=0", productdes).GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject<ResponseData<Bill>>(await response.Content.ReadAsStringAsync());
                return result;
            }

        }

        public async Task<ResponseData<string>> CompleteBooking(ActionView actionView)
        {
            var response = await _httpClient.PatchAsJsonAsync<ActionView>($"/api/Booking/Complete-Booking", actionView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<string>> CompleteBookingDetail(ActionView actionView)
        {
            var response = await _httpClient.PatchAsJsonAsync<ActionView>($"/api/Booking/complete-bookingDetails", actionView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<string>> ConfirmBooking(ActionView actionView)
        {
            var response = await _httpClient.PatchAsJsonAsync<ActionView>($"/api/Booking/Confirm-booking", actionView);
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
        public async Task<ResponseData<List<HistoryBookingVM>>> GetReasonCancelBooking(int id)
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<List<HistoryBookingVM>>>($"/api/Booking/Get-Reason-Cancel-Booking?id={id}");
        }
        public async Task<ResponseData<List<ListBokingDetailInDay>>> ListBookingDetailInDay(int id)
        {
            return _httpClient.GetFromJsonAsync<ResponseData<List<ListBokingDetailInDay>>>($"/api/Booking/List-Booking-Detail-In-Day?id={id}").GetAwaiter().GetResult();
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
            var response = await _httpClient.PatchAsJsonAsync<ActionView>($"/api/Booking/start-booking", actionView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<string>> StartBookingDetail(ActionView actionView)
        {
            var response = await _httpClient.PatchAsJsonAsync<ActionView>($"/api/Booking/start-booking-details", actionView);
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
            return await _httpClient.GetFromJsonAsync<ResponseData<string>>($"/api/Booking/QrCode-CheckIn?idBooking={idBooking}");
        }

        public async Task<ResponseData<string>> QrCodeCheckOut(int idBookingDetail)
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<string>>($"/api/Booking/QrCode-CheckOut?idBookingDetail={idBookingDetail}");

        }

        public async Task<ResponseData<ResponseMomo>> PaymentQr(Payment payment, int? id)
        {
            var response = await _httpClient.PostAsJsonAsync<Payment>($"/api/Booking/Payment-Qr?id={id}", payment);
            var result = JsonConvert.DeserializeObject<ResponseData<ResponseMomo>>(await response.Content.ReadAsStringAsync());
            return result;
        }
        public async Task<ResponseData<string>> PaymentQrVnPay(long totalPrice)
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<string>>($"/api/Booking/Payment-Qr-VnPay?totalPrice={totalPrice}");
        }
        public async Task<ResponseData<string>> CheckInArrive(ActionView actionView)
        {
            var response = await _httpClient.PatchAsJsonAsync<ActionView>($"api/Booking/CheckIn-Booking", actionView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());

            return result;
        }
        public async Task<ResponseData<string>> AddService(AddBookingDetail createBookingDetailRequest)
        {
            var response = await _httpClient.PostAsJsonAsync<AddBookingDetail>("/api/Booking/Add-Service-For-Booking", createBookingDetailRequest);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            return result;
        }
    }
}
