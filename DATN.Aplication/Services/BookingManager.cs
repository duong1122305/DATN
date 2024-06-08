using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.BookingManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
    public class BookingManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingManager(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
        public Task<ResponseData<string>> CreateNewBooking(CreateNewBookingRequest request)
        {
            return null;
        }
    }
}
