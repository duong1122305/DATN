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
    public class BookingManagement : IBookingManagement
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
                        group new { guest.Id, guest.Name, guest.Email, guest.Address, guest.PhoneNumber, booking.BookingTime }
                        by new { guest.Id, guest.Name, guest.Email, guest.Address, guest.PhoneNumber, booking.BookingTime, booking.Status }
                        into view
                        select new BookingView
                        {
                            Id = view.Key.Id,
                            Address = view.Key.Address,
                            BookingTime = view.Key.BookingTime,
                            Email = view.Key.Email,
                            NameGuest = view.Key.Name,
                            PhoneNumber = view.Key.PhoneNumber,
                            Status = view.Key.Status
                        };
            if (query.Count()>0)
            {
                return new ResponseData<List<BookingView>>() { IsSuccess = true, Data = query.ToList() };
            }
            else
            {
                return new ResponseData<List<BookingView>> { IsSuccess = false, Data = new List<BookingView>() , Error="Chưa có dữ liệu"};
            }
        }
        public async Task<ResponseData<List<ListBokingDetailInDay>>> GetListBookingDetailInDay(string idGuest, DateTime date)
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
                        where guest.Id == Guid.Parse(idGuest) && booking.BookingTime.Date.CompareTo(date.Date) == 0
                        group new { guest.Id, guest.Name, guest.Email, guest.Address, guest.PhoneNumber, booking.BookingTime }
                        by new { user.FullName, servicedetail.Price, pet.Name, servicedetail.Description, bookingdetail.Status, bookingdetail.StartDateTime, bookingdetail.EndDateTime, booking.BookingTime }
                        into view
                        select new ListBokingDetailInDay
                        {
                            NameStaffService = view.Key.FullName,
                            ServiceDetaiName = view.Key.Description,
                            PetName = view.Key.Name,
                            BookingTime = view.Key.BookingTime,
                            Price = view.Key.Price,
                            Status = view.Key.Status,
                            EndDate = view.Key.EndDateTime,
                            StartDate=view.Key.StartDateTime,
                        };
            if (query.Count() > 0)
            {
                return new ResponseData<List<ListBokingDetailInDay>>() { IsSuccess = true, Data = query.ToList() };
            }
            else
            {
                return new ResponseData<List<ListBokingDetailInDay>> { IsSuccess = false, Data = new List<ListBokingDetailInDay>(), Error = "Chưa có dữ liệu" };
            }
        }
        public async Task<ResponseData<string>> CreateBookingStore()
        {

        }
    }
}