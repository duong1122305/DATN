using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.DTOs.Statistical;

namespace DATN.ADMIN.IServices
{
	public interface IStatiscalClient
	{
		Task<ResponseData<Statistical>> StatisticalIndex(string? startDate, string? endDate, int type = 1);
		Task<ResponseData<List<StatiscalBill>>> StatisticalBill(string? startDate, string? endDate, int type = 3);
		Task<ResponseData<BookingDataStatiscal>> GetDataBooking(int bookingID);
		Task<ResponseData<List<HistoryBookingVM>>> GetHistoryBooking(int bookingID);
	}
}
