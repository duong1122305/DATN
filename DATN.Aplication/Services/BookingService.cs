using Azure;
using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
	public class BookingService : IBookingService
	{
		private readonly IUnitOfWork _unitOfWork;

		public BookingService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}


		public async Task<ResponseData<string>> CreateBooking(CreateBookingRequest request)
		{
			try
			{
				var newBooking = new Booking() 
				{
					BookingTime = DateTime.Now,
					GuestId = request.GuestId,
					IsAddToSchedule = true,
					Status = request.Status,
				};
				var lstServiceDetails = await _unitOfWork.ServiceDetailRepository.GetAllAsync();

				await _unitOfWork.BookingRepository.AddAsync(newBooking);

				var lstBookingDetail = from bd in request.lstBookingDetail
									   join sd in lstServiceDetails on bd.ServiceDetailId equals sd.Id
									   select new BookingDetail()
									   {
										   BookingId = newBooking.Id,
										   Quantity = bd.Quantity,
										   StaffId = bd.StaffId,
										   StartDateTime = bd.StartDateTime,
										   EndDateTime = bd.EndDateTime, 
										   ServiceDetailId = bd.ServiceDetailId,
										   Price = sd.Price,
										   Status = BookingDetailStatus.IsActive,
										   ComboId= bd.ComboId,
									   };
				newBooking.TotalPrice= lstBookingDetail.Sum(x => x.Price);
				 await _unitOfWork.SaveChangeAsync();

				return new ResponseData<string>("Thêm thành công");
			}
			catch (Exception)
			{
				return new ResponseData<string>(false,"Thêm thất bại");
			}
		}

	
	}
}
