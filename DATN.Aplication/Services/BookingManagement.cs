using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
    public class BookingManagement: IBookingManagement
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public BookingManagement(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<ResponseData<List<BookingView>>> GetListBookingInOneWeek()
        {
            var query = from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                        join bookingdetail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                        on booking.Id equals bookingdetail.BookingId
                        join guest in await _unitOfWork.GuestRepository.GetAllAsync()
                        on booking.GuestId equals guest.Id
                        join user in await _userManager.Users.ToListAsync()
                        on bookingdetail.StaffId equals user.Id
                        join servicedetail in await _unitOfWork.ServiceDetailRepository.GetAllAsync()
                        on bookingdetail.ServiceDetailId equals servicedetail.Id
                        join service in await _unitOfWork.ServiceRepository.GetAllAsync()
                        on servicedetail.ServiceId equals service.Id
                        join pet in await _unitOfWork.PetRepository.GetAllAsync()
                        on bookingdetail.PetId equals pet.Id
                        group new {guest.Id, guest.Name,guest.Email,guest.Address,guest.PhoneNumber,booking.BookingTime}
                        by new { guest.Id, guest.Name, guest.Email, guest.Address, guest.PhoneNumber, booking.BookingTime }
                        into view
                        select new BookingView
                        {
                            Id=view.Key.Id,
                            Address= view.Key.Address,
                            BookingTime= view.Key.BookingTime,
                            Email= view.Key.Email,
                            NameGuest=view.Key.Name,
                            PhoneNumber = view.Key.PhoneNumber
                        };
            return new ResponseData<List<BookingView>>() { IsSuccess = true ,Data=query.ToList()};
        }
        public Task<ResponseData<List<>>>
    }
}