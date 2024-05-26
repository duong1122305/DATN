using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.IRepository
{
	public interface IBookingRepository
	{
		Task<ResponseData<List<BookingViewModel>>> GetAll();

	}
}
