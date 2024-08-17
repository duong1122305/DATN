using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.DTOs.Statistical;
using System.Net.Http;

namespace DATN.ADMIN.Services
{
	public class StatiscalClient : IStatiscalClient
	{
		private readonly HttpClient _httpClient;

		public StatiscalClient(HttpClient client)
		{
			_httpClient = client;
		}

		public async Task<ResponseData<BookingDataStatiscal>> GetDataBooking(int bookingID)
		{
            return await _httpClient.GetFromJsonAsync<ResponseData<BookingDataStatiscal>>($"api/Statistical/get-data-in-bill?bookingID={bookingID}");
        }

		public async Task<ResponseData<List<HistoryBookingVM>>> GetHistoryBooking(int bookingID)
		{
            return await _httpClient.GetFromJsonAsync<ResponseData<List<HistoryBookingVM>>>($"api/Statistical/get-history-bill?bookingID={bookingID}");
        }


        public async Task<ResponseData<List<StatiscalBill>>> StatisticalBill(string? startDate, string? endDate, int type = 1)
        {

            if (startDate != null && endDate != null)
            {
                return await _httpClient.GetFromJsonAsync<ResponseData<List<StatiscalBill>>>($"api/Statistical/get-all-bill?type={type}&startDate={startDate!}&endDate={endDate!}");
            }
            return await _httpClient.GetFromJsonAsync<ResponseData<List<StatiscalBill>>>($"api/Statistical/get-all-bill?type={type}");
        }

        public async Task<ResponseData<Statistical>> StatisticalIndex(string? startDate, string? endDate, int type = 1)
		{
			if (startDate != null&& endDate!=null)
            {
				return await _httpClient.GetFromJsonAsync<ResponseData<Statistical>>($"api/Statistical?type={type}&startDate={startDate!}&endDate={endDate!}");
			}
			return await _httpClient.GetFromJsonAsync<ResponseData<Statistical>>($"api/Statistical?type={type}");
		}

		
	}
}
