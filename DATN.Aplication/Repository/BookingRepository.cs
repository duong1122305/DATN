using DATN.Aplication.IRepository;
using DATN.Data.EF;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Repository
{
	public class BookingRepository : IBookingRepository
	{
		private readonly DATNDbContext _context;

		public BookingRepository(DATNDbContext context)
		{
			_context = context;
		}
		public async Task<ResponseData<List<BookingViewModel>>> GetAll()
		{
			try
			{
				var lstBookingVM = await _context.Bookings.Include(p => p.StaffConfirm).Include(p => p.StaffAtCounter).Include(p => p.StaffUpdate)
					 .Include(p => p.Guest).Include(p => p.TypePayment).Include(p => p.Discount)
					 .Select(x =>
					 new BookingViewModel()
					 {
						 BookingTime = x.BookingTime,
						 ConfirmedTime = x.ConfirmedTime,
						 CustomerArrivalTime = x.CustomerArrivalTime,
						 DesiredStartTime = x.DesiredStartTime,
						 GuestId = x.GuestId,
						 Id = x.Id,
						 PaymentTypeId = x.PaymentTypeId,
						 StaffAtCounterId = x.StaffAtCounterId,
						 UpdatedAt = x.UpdatedAt,
						 Status = x.Status,
						 StaffConfirmId = x.StaffConfirmId,
						 TotalPrice = x.TotalPrice,
						 UpdatedBy = x.UpdatedBy,
						 VoucherId = x.VoucherId,
						 GuestName = x.Guest.Name,
						 PaymentName = x.TypePayment.Name,
						 ReducedAmount = x.ReducedAmount,
						 StaffConfirmName = x.StaffConfirm.FullName,
						 StaffCounterName = x.StaffAtCounter.FullName,
						 UpdateByName = x.StaffUpdate.FullName
					 }
				 ).ToListAsync();
				return new ResponseData<List<BookingViewModel>>()
				{
					IsSuccess = true,
					Error = null,
					Data = lstBookingVM
				};
			}
			catch (Exception ex)
			{
				return new ResponseData<List<BookingViewModel>>()
				{
					IsSuccess = false,
					Error = ex.Message,
				};
			}
		}
	}
}
