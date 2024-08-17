using DATN.Aplication.Services.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.Common.Location;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.DTOs.Statistical;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StatisticalController : ControllerBase
	{
		private readonly IStatisticalService _statistical;

		public StatisticalController(IStatisticalService statistical)
        {
			_statistical = statistical;
		}
        [HttpGet]
		public async Task<ResponseData<Statistical>> StatisticalIndex(DateTime? startDate, DateTime? endDate, int type = 1)
		{
			return await _statistical.StatisticalIndex(startDate,endDate, type);
		}
		[HttpGet("get-all-bill")]
		public async Task<ResponseData<List<StatiscalBill>>> StatisticalBill(DateTime? startDate, DateTime? endDate, int type = 1)
		{
			return await _statistical.StatisticalBill(startDate, endDate, type);
		}
		[HttpGet("get-data-in-bill")]
		public async Task<ResponseData<BookingDataStatiscal>> GetDataBooking(int bookingID)
		{
			return await _statistical.GetDataBooking(bookingID);
		}
		[HttpGet("get-history-bill")]
		public async Task<ResponseData<List<HistoryBookingVM>>> GetHistoryBooking(int bookingID)
		{
			return await _statistical.GetHistoryBooking(bookingID);
		}
	}
}
