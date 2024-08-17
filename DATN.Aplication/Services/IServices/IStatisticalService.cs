using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.DTOs.Statistical;

namespace DATN.Aplication.Services.IServices
{
    public interface IStatisticalService
    {
		Task<ResponseData<Statistical>> StatisticalIndex(DateTime? startDate, DateTime? endDate, int type = 1);
		Task<ResponseData<List<StatiscalBill>>> StatisticalBill(DateTime? startDate, DateTime? endDate, int type = 1);
		Task<ResponseData<BookingDataStatiscal>> GetDataBooking(int bookingID);
		Task<ResponseData<List<HistoryBookingVM>>> GetHistoryBooking(int bookingID);
	}
}