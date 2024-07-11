using DATN.Aplication.Services.IServices;
using DATN.Aplication.System;
using DATN.Data.Entities;
using DATN.Data.Enum;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Action;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.DTOs.ServiceDetail;
using DATN.ViewModels.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private readonly IAuthenticate _user;

        public BookingManagement(IUnitOfWork unitOfWork, UserManager<User> userManager, NotificationHub notificationHub, IAuthenticate authenticate)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _notificationHub = notificationHub;
            _user = authenticate;
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
                            IdGuest = view.Key.Id,
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
                        by new { user.FullName, servicedetail.Price, pet.Name, servicedetail.Description, bookingdetail.Status, bookingdetail.StartDateTime, bookingdetail.EndDateTime, booking.BookingTime, bookingdetail.Id }
                        into view
                        select new ListBokingDetailInDay
                        {
                            IdBookingDetail = view.Key.Id,
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
        public async Task<ResponseData<string>> CreateBookingStore(CreateBookingRequest createBookingRequest, string token)
        {
            if (createBookingRequest.ListIdServiceDetail.Count > 0)
            {
                try
                {
                    var booking = new Booking()
                    {
                        GuestId = createBookingRequest.GuestId,
                        BookingTime = DateTime.Now,
                        PaymentTypeId = createBookingRequest.PaymentTypeId,
                        VoucherId = null,
                        TotalPrice = 0,
                        ReducedAmount = 0,
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
                            EndDateTime = item.EndDateTime.CompareTo(new TimeSpan(23, 30, 00)) > 0 ? DateTime.Now.AddDays(1).Date.AddHours(item.EndDateTime.Hours).AddMinutes(item.EndDateTime.Minutes) : DateTime.Now.Date.AddHours(item.EndDateTime.Hours).AddMinutes(item.EndDateTime.Minutes),
                            StartDateTime = item.EndDateTime.CompareTo(new TimeSpan(23, 30, 00)) > 0 ? DateTime.Now.AddDays(1).Date.AddHours(item.StartDateTime.Hours).AddMinutes(item.StartDateTime.Minutes) : DateTime.Now.Date.AddHours(item.StartDateTime.Hours).AddMinutes(item.StartDateTime.Minutes),
                            Status = BookingDetailStatus.Unfulfilled,
                            ServiceDetailId = item.ServiceDetailId,
                        };
                        list.Add(bookingDetail);
                    }
                    await _unitOfWork.BookingDetailRepository.AddRangeAsync(list);
                    var idUserAction = await _user.GetUserByToken(token);
                    if (idUserAction.IsSuccess)
                    {
                        var history = new HistoryAction()
                        {
                            ActionByID = Guid.Parse(idUserAction.Data),
                            ActionTime = DateTime.Now,
                            ActionID = 12,
                            Description = "Đây là tạo lịch chăm sóc tại quầy",
                            BookingID = queryBooking.FirstOrDefault().Id,
                        };
                        await _unitOfWork.HistoryActionRepository.AddAsync(history);
                        await _unitOfWork.SaveChangeAsync();
                    }
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
        public async Task<ResponseData<Bill>> GetBill(Guid IdGuest, DateTime dateBooking)
        {
            var queryBooking = from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                               join bookingDetail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                               on booking.Id equals bookingDetail.BookingId
                               join user in await _userManager.Users.ToListAsync()
                               on bookingDetail.StaffId equals user.Id
                               join guest in await _unitOfWork.GuestRepository.GetAllAsync()
                               on booking.GuestId equals guest.Id
                               join pet in await _unitOfWork.PetRepository.GetAllAsync()
                               on guest.Id equals pet.OwnerId
                               join serviceDetail in await _unitOfWork.ServiceDetailRepository.GetAllAsync()
                               on bookingDetail.ServiceDetailId equals serviceDetail.Id
                               where booking.BookingTime.Date.CompareTo(dateBooking.Date) == 0 && booking.GuestId == IdGuest
                               && bookingDetail.Status == BookingDetailStatus.Completed
                               select new ServiceDetailView
                               {
                                   IdServiceDetail = serviceDetail.Id,
                                   PetName = pet.Name,
                                   Price = serviceDetail.Price,
                                   NameStaff = user.FullName,
                                   ServiceDetailName = serviceDetail.Description
                               };
            var infoGuest = (from guest in await _unitOfWork.GuestRepository.GetAllAsync()
                             where guest.Id == IdGuest
                             select guest).FirstOrDefault();
            double totalPrice = 0;
            foreach (var item in queryBooking)
            {
                var queryGetTotalPrice = from serviceDetail in await _unitOfWork.ServiceDetailRepository.GetAllAsync()
                                         where serviceDetail.Id == item.IdServiceDetail
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
            var maxMoney = queryCheckVoucherCanApply.FirstOrDefault(c => c.Id == voucherWillUse)?.MaxMoneyDiscount;
            var discount = queryCheckVoucherCanApply.FirstOrDefault(c => c.Id == voucherWillUse)?.DiscountPercent;
            var reduce = voucherWillUse != 0 ? (double)discount * totalPrice / 100 >= maxMoney ? maxMoney : (double)discount * totalPrice / 100 : 0;
            var bill = new Bill()
            {
                Address = infoGuest.Address,
                GuestName = infoGuest.Name,
                DateBooking = DateTime.Now,
                ListServiceBooked = queryBooking.ToList(),
                PhoneNumber = infoGuest.PhoneNumber,
                TotalPrice = totalPrice,
                ReducePrice = reduce.Value,
                TotalPayment = totalPrice - reduce.Value,
                IdVoucher = voucherWillUse,
            };
            if (bill != null)
                return new ResponseData<Bill> { IsSuccess = true, Data = bill };
            else
                return new ResponseData<Bill> { IsSuccess = false, Error = "Không tìm thấy bill" };
        }
        public async Task<ResponseData<string>> CompleteBookingDetail(ActionView actionView)
        {
            var query = (from bookingDetail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                         where bookingDetail.Id == actionView.IdBokingOrDetail
                         select bookingDetail).FirstOrDefault();
            if (query != null)
            {
                query.Status = BookingDetailStatus.Completed;
                await _unitOfWork.BookingDetailRepository.UpdateAsync(query);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Thành công" };
            }
            else
                return new ResponseData<string> { IsSuccess = false, Error = "Không tìm thấy" };
        }
        public async Task<ResponseData<string>> CancelBookingDetail(ActionView actionView)
        {
            var idUserAction = await _user.GetUserByToken(actionView.Token);
            var query = (from bookingDetail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                         where bookingDetail.Id == actionView.IdBokingOrDetail
                         select bookingDetail).FirstOrDefault();
            if (query != null)
            {
                query.Status = BookingDetailStatus.Cancelled;
                HistoryAction historyAction = new HistoryAction()
                {
                    BookingID = query.Id,
                    ActionTime = DateTime.Now,
                    Description = actionView.Reason,
                    ActionID = 14,
                    ActionByID = Guid.Parse(idUserAction.Data)
                };
                await _unitOfWork.BookingDetailRepository.UpdateAsync(query);
                await _unitOfWork.HistoryActionRepository.UpdateAsync(historyAction);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Thành công" };
            }
            else
                return new ResponseData<string> { IsSuccess = false, Error = "Không tìm thấy" };
        }
        public async Task<ResponseData<string>> StartBookingDetail(ActionView actionView)
        {
            var query = (from bookingDetail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                         where bookingDetail.Id == actionView?.IdBokingOrDetail
                         select bookingDetail).FirstOrDefault();
            if (query != null)
            {
                query.Status = BookingDetailStatus.Processing;
                await _unitOfWork.BookingDetailRepository.UpdateAsync(query);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Thành công" };
            }
            else
                return new ResponseData<string> { IsSuccess = false, Error = "Không tìm thấy" };
        }
        public async Task<ResponseData<string>> CancelBooking(ActionView actionView)
        {
            var idUserAction = await _user.GetUserByToken(actionView.Token);
            var query = (from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                         where booking.Id == actionView.IdBokingOrDetail
                         select booking).FirstOrDefault();
            if (query != null)
            {
                query.Status = BookingStatus.StaffCancelled;
                var queryBooking = (from bookingDetail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                                    where bookingDetail.BookingId == query.Id
                                    select bookingDetail).ToList();
                foreach (var item in queryBooking)
                {
                    item.Status = BookingDetailStatus.Cancelled;
                }
                HistoryAction historyAction = new HistoryAction()
                {
                    BookingID = query.Id,
                    ActionTime = DateTime.Now,
                    Description = actionView.Reason,
                    ActionID = 14,
                    ActionByID = Guid.Parse(idUserAction.Data)
                };
                await _unitOfWork.BookingRepository.UpdateAsync(query);
                await _unitOfWork.BookingDetailRepository.UpdateRangeAsync(queryBooking);
                await _unitOfWork.HistoryActionRepository.AddAsync(historyAction);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Thành công" };

            }
            else
                return new ResponseData<string> { IsSuccess = false, Error = "không tìm thấy" };
        }
        public async Task<ResponseData<string>> StartBooking(ActionView actionView)
        {
            var query = (from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                         where booking.Id == actionView.IdBokingOrDetail
                         select booking).FirstOrDefault();
            if (query != null)
            {
                query.Status = BookingStatus.InProgress;
                var queryBooking = (from bookingDetail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                                    where bookingDetail.BookingId == query.Id
                                    select bookingDetail).FirstOrDefault();
                queryBooking.Status = BookingDetailStatus.Processing;
                await _unitOfWork.BookingRepository.UpdateAsync(query);
                await _unitOfWork.BookingDetailRepository.UpdateAsync(queryBooking);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Thành công" };

            }
            else
                return new ResponseData<string> { IsSuccess = false, Error = "không tìm thấy" };
        }
        public async Task<ResponseData<string>> CompleteBooking(ActionView actionView)
        {
            var query = (from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                         where booking.Id == actionView.IdBokingOrDetail
                         select booking).FirstOrDefault();
            if (query != null)
            {
                query.Status = BookingStatus.Completed;
                var queryBooking = (from bookingDetail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                                    where bookingDetail.BookingId == query.Id
                                    select bookingDetail).ToList();
                foreach (var item in queryBooking)
                {
                    item.Status = BookingDetailStatus.Completed;
                }
                await _unitOfWork.BookingRepository.UpdateAsync(query);
                await _unitOfWork.BookingDetailRepository.UpdateRangeAsync(queryBooking);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Thành công" };

            }
            else
                return new ResponseData<string> { IsSuccess = false, Error = "không tìm thấy" };
        }
    }
}