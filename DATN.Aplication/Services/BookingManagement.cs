using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.Data.Enum;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
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
        private readonly NotificationHub _notificationHub;

        public BookingManagement(IUnitOfWork unitOfWork, UserManager<User> userManager, NotificationHub notificationHub)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _notificationHub = notificationHub;
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
            if (query.Count() > 0)
            {
                await _notificationHub.Notification("Có khách đặt lịch confirm đi");
                return new ResponseData<List<BookingView>>() { IsSuccess = true, Data = query.ToList() };
            }
            else
            {
                return new ResponseData<List<BookingView>> { IsSuccess = false, Data = new List<BookingView>(), Error = "Chưa có dữ liệu" };
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
                            StartDate = view.Key.StartDateTime,
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
        public async Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest)
        {
            if (createBookingRequest.ListIdServiceDetail.Count > 0)
            {
                double totalPrice = 0;
                foreach (var item in createBookingRequest.ListIdServiceDetail)
                {
                    var queryGetTotalPrice = from serviceDetail in await _unitOfWork.ServiceDetailRepository.GetAllAsync()
                                             where serviceDetail.Id == item.ServiceDetailId
                                             select serviceDetail;
                    totalPrice += queryGetTotalPrice.FirstOrDefault().Price;
                }
                var queryCheckVoucherCanApply = from voucher in await _unitOfWork.DiscountRepository.GetAllAsync()
                                                where totalPrice >= voucher.MinMoneyApplicable
                                                && voucher.AmountUsed < voucher.Quantity
                                                && voucher.Status == VoucherStatus.GoingOn
                                                select voucher;
                int voucherWillUse = 0;
                if (queryCheckVoucherCanApply.Count() > 1)
                {
                    voucherWillUse = queryCheckVoucherCanApply.FirstOrDefault().Id;
                    foreach (var item in queryCheckVoucherCanApply)
                    {
                        var reducedAmount1 = totalPrice * (double)item.DiscountPercent <= item.MaxMoneyDiscount ? totalPrice * (double)item.DiscountPercent : item.MaxMoneyDiscount;
                        foreach (var item2 in queryCheckVoucherCanApply)
                        {
                            if (item.Id == item.Id)
                            {
                                continue;
                            }
                            else
                            {
                                var reducedAmount2 = totalPrice * (double)item2.DiscountPercent <= item2.MaxMoneyDiscount ? totalPrice * (double)item2.DiscountPercent : item2.MaxMoneyDiscount;
                                if (reducedAmount1 < reducedAmount2)
                                {
                                    voucherWillUse = item2.Id;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (queryCheckVoucherCanApply.Count() == 1)
                    {
                        voucherWillUse = queryCheckVoucherCanApply.FirstOrDefault().Id;
                    }
                }

                try
                {
                    var maxMoney = queryCheckVoucherCanApply.FirstOrDefault(c => c.Id == voucherWillUse)?.MaxMoneyDiscount;
                    var discount = queryCheckVoucherCanApply.FirstOrDefault(c => c.Id == voucherWillUse)?.DiscountPercent;
                    var booking = new Booking()
                    {
                        GuestId = createBookingRequest.GuestId,
                        BookingTime = DateTime.Now,
                        PaymentTypeId = createBookingRequest.PaymentTypeId,
                        VoucherId = voucherWillUse != 0 ? voucherWillUse : null,
                        TotalPrice = totalPrice,
                        ReducedAmount = voucherWillUse != 0 ? totalPrice * (double)discount < maxMoney ? totalPrice * (double)discount : maxMoney : 0,
                        Status = BookingStatus.Confirmed,
                    };
                    await _unitOfWork.BookingRepository.AddAsync(booking);
                    await _unitOfWork.SaveChangeAsync();
                    var queryBooking = from bookingTable in await _unitOfWork.BookingRepository.GetAllAsync()
                                       where bookingTable.GuestId == createBookingRequest.GuestId
                                       && bookingTable.BookingTime.Date.CompareTo(DateTime.Now.Date) == 0
                                       select bookingTable;
                    List<BookingDetail> list = new List<BookingDetail>();
                    foreach (var item in createBookingRequest.ListIdServiceDetail)
                    {
                        var bookingDetail = new BookingDetail()
                        {
                            BookingId = queryBooking.FirstOrDefault().Id,
                            PetId = item.PetId,
                            Price = item.Price,
                            StaffId = item.StaffId,
                            EndDateTime = item.EndDateTime,
                            StartDateTime = item.StartDateTime,
                            Status = BookingDetailStatus.Unfulfilled,
                            ServiceDetailId = item.ServiceDetailId,
                        };
                        list.Add(bookingDetail);
                    }
                    await _unitOfWork.BookingDetailRepository.AddRangeAsync(list);
                    await _notificationHub.Notification("Có khách đặt lịch confirm đi");
                    return new ResponseData<string> { IsSuccess = true, Data = "Đặt lịch thành công" };
                }
                catch (Exception e)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = e.Message };
                }
            }
            else
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Chưa chọn dịch vụ không thể đặt lịch :)))))" };
            }
        }
    }
}