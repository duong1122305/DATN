using Azure;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services.IServices
{
	public interface IBookingService
	{
		Task<ResponseData<string>> CreateBooking(CreateBookingRequest request);
	}
}
