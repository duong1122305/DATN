using DATN.Aplication.Services.IServices;
using DATN.Aplication.System;
using DATN.Data.Entities;
using DATN.Data.Enum;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ActionBooking;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.DTOs.Product;
using DATN.ViewModels.DTOs.ServiceDetail;
using DATN.ViewModels.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.IO;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MessagingToolkit.QRCode.Codec;
using System.Drawing.Imaging;
using ZXing;
using ZXing.QrCode.Internal;
using ZXing.Common;
using static System.Net.WebRequestMethods;
using DATN.ViewModels.DTOs.Payment.DATN.ViewModels.DTOs.Payment;
using DATN.ViewModels.DTOs.Payment;
using System.Buffers.Text;

namespace DATN.Aplication.Services
{
    public class BookingManagement : IBookingManagement
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly NotificationHub _notificationHub;
        private readonly IAuthenticate _user;
        private readonly IProductManagement _productManagement;
        private readonly IEmployeeScheduleManagementService _employeeScheduleManagementService;
        private readonly IVoucherManagementService _voucherManagementService;
        public BookingManagement(IUnitOfWork unitOfWork, UserManager<User> userManager, NotificationHub notificationHub, IAuthenticate authenticate, IProductManagement productManagement, IEmployeeScheduleManagementService employeeScheduleManagementService, IVoucherManagementService voucherManagementService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _notificationHub = notificationHub;
            _user = authenticate;
            _productManagement = productManagement;
            _employeeScheduleManagementService = employeeScheduleManagementService;
            _voucherManagementService = voucherManagementService;
        }
        public async Task ChangeStatusBooking()
        {
            var querybooking = from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                               select booking;
            List<Booking> lstBookingCancel = new List<Booking>();

            List<BookingDetail> lstBookingDetailCancel = new List<BookingDetail>();
            List<HistoryAction> lstAction = new List<HistoryAction>();
            foreach (var item in querybooking)
            {
                if (item.Status == BookingStatus.Confirmed || item.Status == BookingStatus.PendingConfirmation)
                {
                    var queryBookingDetail = from detail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                                             where detail.BookingId == item.Id
                                             select detail;
                    int count = 0;
                    foreach (var item1 in queryBookingDetail)
                    {
                        if (item1.Status != BookingDetailStatus.Cancelled)
                        {
                            if (item1.EndDateTime.CompareTo(DateTime.Now) < 0)
                            {
                                item1.Status = BookingDetailStatus.Cancelled;
                                lstBookingDetailCancel.Add(item1);
                                count++;
                            }
                        }
                    }
                    if (count == queryBookingDetail.Count())
                    {
                        item.Status = BookingStatus.AdminCancelled;
                        lstBookingCancel.Add(item);
                        HistoryAction action = new HistoryAction()
                        {
                            ActionID = 14,
                            ActionTime = DateTime.Now,
                            Description = "Hệ thống hủy lịch đặt do quá hạn mà đơn chưa xác nhận hoặc khách chưa đến sử dụng dịch vụ",
                            BookingID = item.Id,
                        };
                        lstAction.Add(action);
                    }
                }
            }
            if (lstBookingCancel.Count > 0)
            {
                await _unitOfWork.BookingRepository.UpdateRangeAsync(lstBookingCancel);
                await _unitOfWork.HistoryActionRepository.AddRangeAsync(lstAction);

            }
            if (lstBookingDetailCancel.Count > 0)
            {
                await _unitOfWork.BookingDetailRepository.UpdateRangeAsync(lstBookingDetailCancel);
            }
        }
        public async Task<ResponseData<List<BookingView>>> GetListBookingInOneWeek()
        {
            await ChangeStatusBooking();
            var query = from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                        join bookingdetail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                        on booking.Id equals bookingdetail.BookingId
                        join guest in await _unitOfWork.GuestRepository.GetAllAsync()
                        on booking.GuestId equals guest.Id
                        group new { guest.Id, guest.Name, guest.Email, guest.Address, guest.PhoneNumber, booking.BookingTime }
                        by new { guest.Id, guest.Name, guest.Email, guest.Address, guest.PhoneNumber, booking.BookingTime, booking.Status, bookingdetail.BookingId, booking.IsPayment }
                        into view
                        select new BookingView
                        {
                            IdBooking = view.Key.BookingId,
                            IdGuest = view.Key.Id,
                            Address = view.Key.Address,
                            BookingTime = view.Key.BookingTime,
                            Email = view.Key.Email,
                            NameGuest = view.Key.Name,
                            PhoneNumber = view.Key.PhoneNumber,
                            Status = view.Key.Status,
                            IsPayment = view.Key.IsPayment
                        };
            if (query.Count() > 0)
            {
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
        public async Task<ResponseData<string>> CreateBookingInStore(CreateBookingRequest createBookingRequest, string token)
        {
            if (createBookingRequest.ListIdServiceDetail.Count > 0)
            {
                try
                {
                    var queryBooking = from bookingTable in await _unitOfWork.BookingRepository.GetAllAsync()
                                       where bookingTable.GuestId == createBookingRequest.GuestId
                                       && bookingTable.BookingTime.Date.CompareTo(DateTime.Now.Date) == 0
                                       && bookingTable.Status != BookingStatus.StaffCancelled
                                       && bookingTable.Status != BookingStatus.AdminCancelled
                                       && bookingTable.Status != BookingStatus.CustomerCancelled
                                       && bookingTable.Status != BookingStatus.NoShow
                                       && bookingTable.Status != BookingStatus.Confirmed
                                       select bookingTable;
                    if (queryBooking.Count() == 0)
                    {
                        var booking = new Booking()
                        {
                            GuestId = createBookingRequest.GuestId,
                            BookingTime = DateTime.Now,
                            VoucherId = null,
                            TotalPrice = 0,
                            PaymentTypeId = 1,
                            ReducedAmount = 0,
                            Status = BookingStatus.Confirmed,
                            IsPayment = false,
                            IsAddToSchedule = true,
                        };
                        await _unitOfWork.BookingRepository.AddAsync(booking);
                        await _unitOfWork.SaveChangeAsync();
                        queryBooking = from bookingTable in await _unitOfWork.BookingRepository.GetAllAsync()
                                       where bookingTable.GuestId == createBookingRequest.GuestId
                                       && bookingTable.BookingTime.Date.CompareTo(DateTime.Now.Date) == 0
                                       && bookingTable.Status != BookingStatus.StaffCancelled
                                       && bookingTable.Status != BookingStatus.AdminCancelled
                                       && bookingTable.Status != BookingStatus.CustomerCancelled
                                       && bookingTable.Status != BookingStatus.NoShow
                                       && bookingTable.Status != BookingStatus.Confirmed
                                       select bookingTable;
                    }
                    else
                    {
                        var queryBookingDetailHaven = (from bookingDetail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                                                       where bookingDetail.BookingId == queryBooking.FirstOrDefault().Id
                                                       select bookingDetail).ToList();
                        for (global::System.Int32 i = 0; i < createBookingRequest.ListIdServiceDetail.Count; i++)
                        {
                            for (global::System.Int32 j = 0; j < queryBookingDetailHaven.Count(); j++)
                            {
                                var term1 = createBookingRequest.ListIdServiceDetail[i];
                                var term2 = queryBookingDetailHaven[j];
                                if (term1.ServiceDetailId == term2.ServiceDetailId && term1.PetId == term2.PetId)
                                {
                                    return new ResponseData<string> { IsSuccess = false, Error = "Trong các dịch vụ đã chọn có 1 dịch vụ sử dùng 2 lần cho 1 bé thú cưng!!!" };
                                }
                                else if (term1.ServiceDetailId != term2.ServiceDetailId && term1.StaffId == term2.StaffId)
                                {
                                    if (term1.StartDateTime.CompareTo(term2.StartDateTime.TimeOfDay) > 0 && term1.StartDateTime.CompareTo(term2.EndDateTime.TimeOfDay) < 0)
                                    {
                                        return new ResponseData<string> { IsSuccess = false, Error = "Trong các dịch vụ đã chọn có dịch vụ chung 1 người làm và cùng 1 thời điểm!!!" };
                                    }
                                    else if (term1.EndDateTime.CompareTo(term2.StartDateTime.TimeOfDay) > 0 && term1.EndDateTime.CompareTo(term2.EndDateTime.TimeOfDay) < 0)
                                    {
                                        return new ResponseData<string> { IsSuccess = false, Error = "Trong các dịch vụ đã chọn có dịch vụ chung 1 người làm và cùng 1 thời điểm!!!" };
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                    }

                    List<BookingDetail> list = new List<BookingDetail>();
                    var queryServiceDetail = from detail in await _unitOfWork.ServiceDetailRepository.GetAllAsync()
                                             select detail;
                    for (global::System.Int32 i = 0; i < createBookingRequest.ListIdServiceDetail.Count; i++)
                    {
                        for (global::System.Int32 j = 0; j < createBookingRequest.ListIdServiceDetail.Count; j++)
                        {
                            if (i == j)
                            {
                                continue;
                            }
                            else
                            {
                                var term1 = createBookingRequest.ListIdServiceDetail[i];
                                var term2 = createBookingRequest.ListIdServiceDetail[j];
                                if (term1.ServiceDetailId == term2.ServiceDetailId && term1.PetId == term2.PetId)
                                {
                                    return new ResponseData<string> { IsSuccess = false, Error = "Trong các dịch vụ đã chọn có 1 dịch vụ sử dùng 2 lần cho 1 bé thú cưng!!!" };
                                }
                                else if (term1.ServiceDetailId == term2.ServiceDetailId && term1.StaffId == term2.StaffId)
                                {
                                    if (term1.StartDateTime.CompareTo(term2.StartDateTime) > 0 && term1.StartDateTime.CompareTo(term2.EndDateTime) < 0)
                                    {
                                        return new ResponseData<string> { IsSuccess = false, Error = "Trong các dịch vụ đã chọn có dịch vụ chung 1 người làm và cùng 1 thời điểm!!!" };
                                    }
                                    else if (term1.EndDateTime.CompareTo(term2.StartDateTime) > 0 && term1.EndDateTime.CompareTo(term2.EndDateTime) < 0)
                                    {
                                        return new ResponseData<string> { IsSuccess = false, Error = "Trong các dịch vụ đã chọn có dịch vụ chung 1 người làm và cùng 1 thời điểm!!!" };
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    foreach (var item in createBookingRequest.ListIdServiceDetail)
                    {
                        var bookingDetail = new BookingDetail()
                        {
                            BookingId = queryBooking.FirstOrDefault().Id,
                            PetId = item.PetId,
                            Price = item.Price,
                            StaffId = item.StaffId,
                            EndDateTime = item.DateBooking.Date.AddHours(item.EndDateTime.Hours).AddMinutes(item.EndDateTime.Minutes),
                            StartDateTime = item.DateBooking.Date.AddHours(item.StartDateTime.Hours).AddMinutes(item.StartDateTime.Minutes),
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
        public async Task<ResponseData<string>> GuestCreateBooking(CreateBookingRequest createBookingRequest)
        {

            if (createBookingRequest.ListIdServiceDetail.Count > 0)
            {
                try
                {
                    var query = from serviceDetail in await _unitOfWork.ServiceDetailRepository.GetAllAsync()
                                select serviceDetail;
                    for (global::System.Int32 i = 0; i < createBookingRequest.ListIdServiceDetail.Count; i++)
                    {
                        if (i == 0)
                        {
                            int time = (int)(query.FirstOrDefault(c => c.Id == createBookingRequest.ListIdServiceDetail[0].ServiceDetailId).Duration) / 60;
                            createBookingRequest.ListIdServiceDetail[0].EndDateTime = createBookingRequest.ListIdServiceDetail[0].StartDateTime.Add(new TimeSpan(time, (int)(query.FirstOrDefault(c => c.Id == createBookingRequest.ListIdServiceDetail[0].ServiceDetailId).Duration - (time * 60)), 0));
                        }
                        else
                        {
                            int time = (int)(query.FirstOrDefault(c => c.Id == createBookingRequest.ListIdServiceDetail[0].ServiceDetailId).Duration) / 60;
                            createBookingRequest.ListIdServiceDetail[i].StartDateTime = createBookingRequest.ListIdServiceDetail[i - 1].EndDateTime;
                            createBookingRequest.ListIdServiceDetail[i].EndDateTime = createBookingRequest.ListIdServiceDetail[i].StartDateTime.Add(new TimeSpan(time, (int)(query.FirstOrDefault(c => c.Id == createBookingRequest.ListIdServiceDetail[i].ServiceDetailId).Duration - (time * 60)), 0));
                        }
                    }

                    foreach (var item in createBookingRequest.ListIdServiceDetail)
                    {
                        var lst = await _employeeScheduleManagementService.ListStaffFreeInTime(item.StartDateTime, item.EndDateTime, item.DateBooking.Date);
                        if (lst.IsSuccess)
                        {
                            item.StaffId = lst.Data.FirstOrDefault().IdStaff;
                        }
                        else
                        {
                            return new ResponseData<string> { IsSuccess = false, Error = lst.Error };
                        }
                    }

                    var queryBooking = from bookingTable in await _unitOfWork.BookingRepository.GetAllAsync()
                                       where bookingTable.GuestId == createBookingRequest.GuestId
                                       && bookingTable.BookingTime.Date.CompareTo(DateTime.Now.Date) == 0
                                       && bookingTable.Status != BookingStatus.StaffCancelled && bookingTable.Status != BookingStatus.AdminCancelled && bookingTable.Status != BookingStatus.CustomerCancelled
                                       select bookingTable;
                    if (queryBooking.Count() == 0)
                    {
                        var booking = new Booking()
                        {
                            GuestId = createBookingRequest.GuestId,
                            BookingTime = DateTime.Now,
                            VoucherId = createBookingRequest.VoucherId,
                            TotalPrice = 0,
                            PaymentTypeId = 1,
                            ReducedAmount = 0,
                            Status = BookingStatus.PendingConfirmation,
                            IsPayment = false,
                            IsAddToSchedule = false,
                        };
                        await _unitOfWork.BookingRepository.AddAsync(booking);
                        await _unitOfWork.SaveChangeAsync();
                        queryBooking = from bookingTable in await _unitOfWork.BookingRepository.GetAllAsync()
                                       where bookingTable.GuestId == createBookingRequest.GuestId
                                       && bookingTable.BookingTime.Date.CompareTo(DateTime.Now.Date) == 0
                                       && bookingTable.Status != BookingStatus.StaffCancelled && bookingTable.Status != BookingStatus.AdminCancelled && bookingTable.Status != BookingStatus.CustomerCancelled
                                       select bookingTable;
                    }
                    else
                    {
                        var queryBookingDetailHaven = (from bookingDetail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                                                       where bookingDetail.BookingId == queryBooking.FirstOrDefault().Id
                                                       select bookingDetail).ToList();
                        for (global::System.Int32 i = 0; i < createBookingRequest.ListIdServiceDetail.Count; i++)
                        {
                            for (global::System.Int32 j = 0; j < queryBookingDetailHaven.Count(); j++)
                            {
                                var term1 = createBookingRequest.ListIdServiceDetail[i];
                                var term2 = queryBookingDetailHaven[j];
                                if (term1.ServiceDetailId == term2.ServiceDetailId && term1.PetId == term2.PetId)
                                {
                                    return new ResponseData<string> { IsSuccess = false, Error = "Trong các dịch vụ đã chọn có 1 dịch vụ sử dùng 2 lần cho 1 bé thú cưng!!!" };
                                }
                                else if (term1.ServiceDetailId != term2.ServiceDetailId && term1.StaffId == term2.StaffId)
                                {
                                    if (term1.StartDateTime.CompareTo(term2.StartDateTime.TimeOfDay) > 0 && term1.StartDateTime.CompareTo(term2.EndDateTime.TimeOfDay) < 0)
                                    {
                                        return new ResponseData<string> { IsSuccess = false, Error = "Trong các dịch vụ đã chọn có dịch vụ chung 1 người làm và cùng 1 thời điểm!!!" };
                                    }
                                    else if (term1.EndDateTime.CompareTo(term2.StartDateTime.TimeOfDay) > 0 && term1.EndDateTime.CompareTo(term2.EndDateTime.TimeOfDay) < 0)
                                    {
                                        return new ResponseData<string> { IsSuccess = false, Error = "Trong các dịch vụ đã chọn có dịch vụ chung 1 người làm và cùng 1 thời điểm!!!" };
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                    }

                    List<BookingDetail> list = new List<BookingDetail>();
                    var queryServiceDetail = from detail in await _unitOfWork.ServiceDetailRepository.GetAllAsync()
                                             select detail;
                    for (global::System.Int32 i = 0; i < createBookingRequest.ListIdServiceDetail.Count; i++)
                    {
                        for (global::System.Int32 j = 0; j < createBookingRequest.ListIdServiceDetail.Count; j++)
                        {
                            if (i == j)
                            {
                                continue;
                            }
                            else
                            {
                                var term1 = createBookingRequest.ListIdServiceDetail[i];
                                var term2 = createBookingRequest.ListIdServiceDetail[j];
                                if (term1.ServiceDetailId == term2.ServiceDetailId && term1.PetId == term2.PetId)
                                {
                                    return new ResponseData<string> { IsSuccess = false, Error = "Trong các dịch vụ đã chọn có 1 dịch vụ sử dùng 2 lần cho 1 bé thú cưng!!!" };
                                }
                                else if (term1.ServiceDetailId == term2.ServiceDetailId && term1.StaffId == term2.StaffId)
                                {
                                    if (term1.StartDateTime.CompareTo(term2.StartDateTime) > 0 && term1.StartDateTime.CompareTo(term2.EndDateTime) < 0)
                                    {
                                        return new ResponseData<string> { IsSuccess = false, Error = "Trong các dịch vụ đã chọn có dịch vụ chung 1 người làm và cùng 1 thời điểm!!!" };
                                    }
                                    else if (term1.EndDateTime.CompareTo(term2.StartDateTime) > 0 && term1.EndDateTime.CompareTo(term2.EndDateTime) < 0)
                                    {
                                        return new ResponseData<string> { IsSuccess = false, Error = "Trong các dịch vụ đã chọn có dịch vụ chung 1 người làm và cùng 1 thời điểm!!!" };
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    foreach (var item in createBookingRequest.ListIdServiceDetail)
                    {
                        var bookingDetail = new BookingDetail()
                        {
                            BookingId = queryBooking.FirstOrDefault().Id,
                            PetId = item.PetId,
                            Price = queryServiceDetail.FirstOrDefault(c => c.Id == item.ServiceDetailId).Price,
                            StaffId = item.StaffId,
                            EndDateTime = item.DateBooking.Date.AddHours(item.EndDateTime.Hours).AddMinutes(item.EndDateTime.Minutes),
                            StartDateTime = item.DateBooking.Date.AddHours(item.StartDateTime.Hours).AddMinutes(item.StartDateTime.Minutes),
                            Status = BookingDetailStatus.Unfulfilled,
                            ServiceDetailId = item.ServiceDetailId,
                        };
                        list.Add(bookingDetail);
                    }
                    await _unitOfWork.BookingDetailRepository.AddRangeAsync(list);
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
            var query = (from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                         where booking.GuestId == IdGuest && booking.BookingTime.Date.CompareTo(dateBooking.Date) == 0
                         select booking).FirstOrDefault();
            var billproduct = await _productManagement.GetBillProduct(query.Id);
            totalPrice += billproduct != null ? billproduct.Data.TotalPrice : 0d;
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
                ListProductDetail = billproduct.Data.ListProductDetail,
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
            var idUser = await _user.GetUserByToken(actionView.Token);
            if (query != null)
            {
                if (query.Status == BookingDetailStatus.Processing)
                {
                    var queryBoking = (from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                                       where booking.Id == query.BookingId
                                       select booking).FirstOrDefault();
                    HistoryAction historyAction = new HistoryAction()
                    {
                        ActionByID = Guid.Parse(idUser.Data),
                        ActionID = 15,
                        ActionTime = DateTime.Now,
                        BookingID = queryBoking.Id,
                        Description = "Đây là hoàn thành 1 dịch vụ con của dịch vụ"
                    };
                    query.Status = BookingDetailStatus.Completed;
                    await _unitOfWork.BookingDetailRepository.UpdateAsync(query);
                    await _unitOfWork.HistoryActionRepository.AddAsync(historyAction);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Thành công" };
                }
                else
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Dịch vụ trong trạng thái không thể hoàn thành" };
                }
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
                int count = 0;
                foreach (var item in (await _unitOfWork.BookingDetailRepository.GetAllAsync()).Where(c => c.BookingId == query.BookingId && c.Status != BookingDetailStatus.Cancelled))
                {
                    count++;
                }
                if (count > 1)
                {

                    if (query.Status == BookingDetailStatus.Processing || query.Status == BookingDetailStatus.Completed)
                    {
                        return new ResponseData<string> { IsSuccess = false, Error = "Trang thái của dịch vụ hiện tại không cho phép hủy " };
                    }
                    else
                    {
                        try
                        {
                            var queryBoking = (from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                                               where booking.Id == query.BookingId
                                               select booking).FirstOrDefault();
                            query.Status = BookingDetailStatus.Cancelled;
                            HistoryAction historyAction = new HistoryAction()
                            {
                                BookingID = queryBoking.Id,
                                ActionTime = DateTime.Now,
                                Description = actionView.Reason + " (Đây là hủy 1 dịch vụ con)",
                                ActionID = 14,
                                ActionByID = Guid.Parse(idUserAction.Data)
                            };
                            await _unitOfWork.BookingDetailRepository.UpdateAsync(query);
                            await _unitOfWork.HistoryActionRepository.UpdateAsync(historyAction);
                            await _unitOfWork.SaveChangeAsync();
                            return new ResponseData<string> { IsSuccess = true, Data = "Thành công" };
                        }
                        catch (Exception)
                        {
                            return new ResponseData<string> { IsSuccess = false, Error = "Thất bại" };
                        }
                    }

                }
                return new ResponseData<string> { IsSuccess = false, Error = "Chỉ có 1 dịch vụ hủy thì ra booking mà hủy hẳn :)))" };
            }
            else
                return new ResponseData<string> { IsSuccess = false, Error = "Không tìm thấy" };
        }
        public async Task<ResponseData<string>> StartBookingDetail(ActionView actionView)
        {
            var query = (from bookingDetail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                         where bookingDetail.Id == actionView?.IdBokingOrDetail
                         select bookingDetail).FirstOrDefault();
            if (query.StartDateTime.Date.CompareTo(DateTime.Now.Date) != 0)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Dịch vụ này không phải đặt cho hôm nay ko thực hiện được mất rùi !!!!" };
            }
            else
            {
                var queryBooking = (from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                                    where booking.Id == query.BookingId
                                    select booking).FirstOrDefault();
                if (queryBooking.Status == BookingStatus.InProgress)
                {
                    if (query != null)
                    {
                        if (query.Status == BookingDetailStatus.Cancelled)
                        {
                            return new ResponseData<string> { IsSuccess = false, Error = "Dịch vụ hủy không bắt đầu được" };
                        }
                        else if (query.Status == BookingDetailStatus.Completed)
                        {
                            return new ResponseData<string> { IsSuccess = false, Error = "Dịch vụ đã hoàn thành không bắt đầu được" };
                        }
                        else
                        {
                            query.Status = BookingDetailStatus.Processing;
                            await _unitOfWork.BookingDetailRepository.UpdateAsync(query);
                            await _unitOfWork.SaveChangeAsync();
                            return new ResponseData<string> { IsSuccess = true, Data = "Thành công" };
                        }
                    }
                    else
                        return new ResponseData<string> { IsSuccess = false, Error = "Không tìm thấy" };
                }
                else
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Bạn chưa bắt đầu làm dịch vụ cho khách không thể bắt đầu dịch vụ con" };
                }
            }
        }
        public async Task<ResponseData<string>> CancelBooking(ActionView actionView)
        {
            var idUserAction = await _user.GetUserByToken(actionView.Token);
            var query = (from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                         where booking.Id == actionView.IdBokingOrDetail
                         select booking).FirstOrDefault();
            if (query != null)
            {
                if (query.Status == BookingStatus.NoShow || query.Status == BookingStatus.PendingConfirmation)
                {
                    try
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
                    catch (Exception)
                    {
                        return new ResponseData<string> { IsSuccess = false, Data = "Thất bại" };
                    }
                }
                else
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Trạng thái hiện tại không hủy được" };
                }
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
                try
                {
                    if (query.Status != BookingStatus.Arrived)
                    {
                        return new ResponseData<string> { IsSuccess = false, Error = "Không thể bắt đầu dịch vụ trong trạng thái này" };
                    }
                    else
                    {
                        query.Status = BookingStatus.InProgress;
                        var queryBooking = from bookingDetail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                                           where bookingDetail.BookingId == query.Id
                                           && bookingDetail.Status == BookingDetailStatus.Unfulfilled
                                           select bookingDetail;
                        var listBookingInDay = queryBooking.Where(c => c.StartDateTime.Date.CompareTo(DateTime.Now.Date) == 0);
                        if (listBookingInDay.Count() == 0)
                        {
                            return new ResponseData<string> { IsSuccess = false, Error = "Khách đặt dịch vụ không phải hôm nay ko thể bắt đầu" };
                        }
                        else
                        {
                            foreach (var item in listBookingInDay.OrderBy(c => c.StartDateTime))
                            {
                                if (DateTime.Now.TimeOfDay.CompareTo(item.StartDateTime.TimeOfDay) >= 0 && DateTime.Now.TimeOfDay.CompareTo(item.EndDateTime.TimeOfDay) <= 0)
                                {
                                    var update = new BookingDetail();
                                    foreach (var item1 in queryBooking)
                                    {
                                        foreach (var item2 in queryBooking)
                                        {
                                            if (item2.StartDateTime.CompareTo(item1.StartDateTime) < 0)
                                            {
                                                update = item2;
                                            }
                                        }
                                    }
                                    await _unitOfWork.BookingDetailRepository.UpdateAsync(update);
                                    await _unitOfWork.BookingRepository.UpdateAsync(query);
                                    await _unitOfWork.SaveChangeAsync();
                                    return new ResponseData<string> { IsSuccess = true, Data = "Thành công" };
                                }
                            }
                            return new ResponseData<string> { IsSuccess = false, Error = "Chưa đến giờ mà khách đặt dịch vụ" };
                        }
                    }
                }
                catch (Exception)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Thất bại" };
                }

            }
            else
                return new ResponseData<string> { IsSuccess = false, Error = "không tìm thấy" };
        }
        public async Task<ResponseData<string>> CompleteBooking(ActionView actionView)
        {
            var query = (from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                         where booking.Id == actionView.IdBokingOrDetail
                         select booking).FirstOrDefault();
            var idUser = await _user.GetUserByToken(actionView.Token);
            if (query != null)
            {
                if (query.Status == BookingStatus.InProgress)
                {
                    query.Status = BookingStatus.Completed;
                    var queryBooking = (from bookingDetail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                                        where bookingDetail.BookingId == query.Id
                                        select bookingDetail).ToList();
                    foreach (var item in queryBooking)
                    {
                        if (item.Status != BookingDetailStatus.Cancelled)
                        {
                            item.Status = BookingDetailStatus.Completed;
                        }
                        if (item.Status == BookingDetailStatus.Unfulfilled)
                        {
                            return new ResponseData<string> { IsSuccess = false, Error = "Có dịch vụ chưa được thực hiện không thể hoàn thành" };
                        }
                    }
                    HistoryAction historyAction = new HistoryAction()
                    {
                        ActionByID = Guid.Parse(idUser.Data),
                        ActionID = 15,
                        ActionTime = DateTime.Now,
                        BookingID = query.Id,
                        Description = "Đây là hoàn thành toàn bộ dịch vụ"
                    };
                    await _unitOfWork.BookingRepository.UpdateAsync(query);
                    await _unitOfWork.BookingDetailRepository.UpdateRangeAsync(queryBooking);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Thành công" };
                }
                return new ResponseData<string> { IsSuccess = false, Error = "Trạng thái của dịch vụ không thể hoàn thành" };

            }
            else
                return new ResponseData<string> { IsSuccess = false, Error = "không tìm thấy" };
        }
        public async Task<ResponseData<string>> ConfirmBooking(ActionView actionView)
        {
            var query = (from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                         where booking.Id == actionView.IdBokingOrDetail
                         select booking).FirstOrDefault();
            if (query != null)
            {
                try
                {
                    if (query.Status != BookingStatus.PendingConfirmation)
                    {
                        return new ResponseData<string> { IsSuccess = false, Error = "Không thể xác nhận dịch vụ có trạng thái này" };
                    }
                    else
                    {
                        var queryBookingDetail = from bkd in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                                                 where bkd.BookingId == actionView.IdBokingOrDetail
                                                 select bkd;
                        var queryVoucher = (from dis in await _unitOfWork.DiscountRepository.GetAllAsync()
                                            where dis.Id == query.VoucherId
                                            select dis).FirstOrDefault();
                        var totalPrice = 0d;
                        foreach (var item in queryBookingDetail)
                        {
                            totalPrice += item.Price;
                        }

                        query.TotalPrice = totalPrice;
                        if (queryVoucher != null)
                        {
                            queryVoucher.AmountUsed++;
                            query.ReducedAmount = totalPrice * (double)queryVoucher.DiscountPercent <= queryVoucher.MaxMoneyDiscount ? totalPrice * (double)queryVoucher.DiscountPercent : queryVoucher.MaxMoneyDiscount;
                            await _unitOfWork.DiscountRepository.UpdateAsync(queryVoucher);
                        }
                        HistoryAction historyAction = new HistoryAction()
                        {
                            ActionByID = Guid.Parse((await _user.GetUserByToken(actionView.Token)).Data),
                            ActionTime = DateTime.Now,
                            BookingID = query.Id,
                            ActionID = 12,
                            Description = "Xác nhận đặt lịch online của khách",
                        };
                        query.Status = BookingStatus.Confirmed;
                        await _unitOfWork.BookingRepository.UpdateAsync(query);
                        await _unitOfWork.HistoryActionRepository.AddAsync(historyAction);
                        await _unitOfWork.SaveChangeAsync();
                        return new ResponseData<string> { IsSuccess = true, Data = "Thành công" };
                    }
                }
                catch (Exception)
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Thất bại" };
                }

            }
            else
                return new ResponseData<string> { IsSuccess = false, Error = "không tìm thấy" };
        }
        public async Task<ResponseData<string>> PaymentInStore(Payment payment)
        {
            var queryBooking = (from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                                where booking.Id == payment.IdBooking
                                select booking).FirstOrDefault();
            var bill = await CheckBill(payment.IdBooking, payment.LstProducts);

            if (queryBooking != null)
            {
                if (!queryBooking.IsPayment)
                {
                    if (payment.TypePaymenId == 1)
                    {
                        if (queryBooking.VoucherId != null)
                        {
                            var checkVoucher = (from dis in await _unitOfWork.DiscountRepository.GetAllAsync()
                                                where dis.Id == queryBooking.VoucherId
                                                select dis).FirstOrDefault();
                            checkVoucher.AmountUsed++;
                            await _unitOfWork.DiscountRepository.UpdateAsync(checkVoucher);

                        }
                        queryBooking.TotalPrice = bill.Data.TotalPayment;
                        queryBooking.PaymentTypeId = payment.TypePaymenId;
                        queryBooking.ReducedAmount = bill.Data.ReducePrice;
                        queryBooking.IsPayment = true;
                        queryBooking.VoucherId = bill.Data.IdVoucher;
                        var listProduct = (from buypro in payment.LstProducts
                                           select new BuyProduct
                                           {
                                               IdBooking = payment.IdBooking,
                                               IdProductDetail = buypro.IdProductDetail,
                                               Quantity = buypro.Quantity,
                                               Price = buypro.Price,
                                           }).ToList();

                        await _unitOfWork.BookingRepository.UpdateAsync(queryBooking);
                        await _productManagement.BuyProduct(listProduct);
                        await _unitOfWork.SaveChangeAsync();
                        return new ResponseData<string>() { IsSuccess = true, Data = "Thanh toán thành công" };
                    }
                    else
                    {
                        return new ResponseData<string> { IsSuccess = true, Data = "Chưa biết làm chuyển khoản" };
                    }
                }
                else
                {
                    return new ResponseData<string> { IsSuccess = false, Error = "Đơn này thanh toán rồi thanh toán đéo gì lắm" };
                }
            }
            else
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Ngày này làm gì có khách này đặt mà thanh toán :)))))" };
            }
        }
        public async Task<ResponseData<Bill>> CheckBill(int? idBooking, List<ProductDetailView> productdes)
        {
            var totalprice = 0d;
            var info = new Guest();
            var queryBooking = new List<ServiceDetailView>();
            if (idBooking != null)
            {
                queryBooking = (from booking in await _unitOfWork.BookingRepository.GetAllAsync()
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
                                where booking.Id == idBooking.Value
                                && bookingDetail.Status == BookingDetailStatus.Completed
                                select new ServiceDetailView
                                {
                                    IdServiceDetail = serviceDetail.Id,
                                    PetName = pet.Name,
                                    Price = serviceDetail.Price,
                                    NameStaff = user.FullName,
                                    ServiceDetailName = serviceDetail.Description
                                }).ToList();
                foreach (var item in queryBooking)
                {
                    totalprice += item.Price;
                }
                info = (from guest in await _unitOfWork.GuestRepository.GetAllAsync()
                        join booking in await _unitOfWork.BookingRepository.GetAllAsync()
                        on guest.Id equals booking.GuestId
                        where booking.Id == idBooking.Value
                        select guest).FirstOrDefault();
            }
            if (productdes.Count > 0)
            {
                foreach (var item in productdes)
                {
                    totalprice += item.Price * item.SelectQuantityProduct;
                }
            }
            if (totalprice != 0)
            {
                var queryCheckVoucherCanApply = (await _voucherManagementService.GetAllVoucherCanApply(totalprice)).Data;
                int voucherWillUse = 0;
                if (queryCheckVoucherCanApply.Count > 1)
                {
                    voucherWillUse = queryCheckVoucherCanApply.FirstOrDefault().Id.Value;
                    foreach (var item in queryCheckVoucherCanApply)
                    {
                        var reducedAmount1 = totalprice * (double)item.DiscountPercent <= item.MaxMoneyDiscount ? totalprice * (double)item.DiscountPercent : item.MaxMoneyDiscount;
                        foreach (var item2 in queryCheckVoucherCanApply)
                        {
                            if (item.Id == item.Id)
                            {
                                continue;
                            }
                            else
                            {
                                var reducedAmount2 = totalprice * (double)item2.DiscountPercent <= item2.MaxMoneyDiscount ? totalprice * (double)item2.DiscountPercent : item2.MaxMoneyDiscount;
                                if (reducedAmount1 < reducedAmount2)
                                {
                                    voucherWillUse = item2.Id.Value;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (queryCheckVoucherCanApply.Count() == 1)
                    {
                        voucherWillUse = queryCheckVoucherCanApply.FirstOrDefault().Id.Value;
                    }
                }
                var maxMoney = queryCheckVoucherCanApply.FirstOrDefault(c => c.Id == voucherWillUse)?.MaxMoneyDiscount;
                var discount = queryCheckVoucherCanApply.FirstOrDefault(c => c.Id == voucherWillUse)?.DiscountPercent;
                var reduce = voucherWillUse != 0 ? (double)discount * totalprice / 100 >= maxMoney ? maxMoney : (double)discount * totalprice / 100 : 0;
                if (queryBooking.Count > 0)
                {
                    return new ResponseData<Bill>
                    {
                        IsSuccess = true,
                        Data = new Bill()
                        {
                            TotalPrice = totalprice,
                            DateBooking = DateTime.Now,
                            IdVoucher = voucherWillUse != 0 ? voucherWillUse : null,
                            ReducePrice = reduce.Value,
                            TotalPayment = totalprice - reduce.Value,
                            ListProductDetail = productdes,
                            GuestName = idBooking != null ? info.Name : "Khách lẻ",
                            Address = idBooking != null ? info.Address : "Không có",
                            PhoneNumber = idBooking != null ? info.PhoneNumber : "Không có",
                            ListServiceBooked = queryBooking.ToList(),
                        }
                    };
                }
                else
                {
                    return new ResponseData<Bill>
                    {
                        IsSuccess = true,
                        Data = new Bill()
                        {
                            TotalPrice = totalprice,
                            DateBooking = DateTime.Now,
                            IdVoucher = voucherWillUse != 0 ? voucherWillUse : null,
                            ReducePrice = reduce.Value,
                            TotalPayment = totalprice - reduce.Value,
                            ListProductDetail = productdes,
                            GuestName = idBooking != null ? info.Name : "Khách lẻ",
                            Address = idBooking != null ? info.Address : "Không có",
                            PhoneNumber = idBooking != null ? info.PhoneNumber : "Không có",
                        }
                    };
                }
            }
            else
                return new ResponseData<Bill> { IsSuccess = false, Error = "Tông tiền ko có gì" };
        }
        public async Task<ResponseData<string>> QrCodeCheckIn(int idBooking)
        {
            if (idBooking != null)
            {
                try
                {
                    BarcodeWriter<Bitmap> barcodeWriter = new BarcodeWriter<Bitmap>();
                    barcodeWriter.Format = BarcodeFormat.QR_CODE;
                    barcodeWriter.Options = new EncodingOptions { Height = 200, Width = 200 };
                    var qrCodeImage = barcodeWriter.Write("https://localhost:7039/swagger/index.html");

                    using (MemoryStream ms = new MemoryStream())
                    {
                        qrCodeImage.Save(ms, ImageFormat.Png);
                        byte[] imageBytes = ms.ToArray();
                        string base64String = Convert.ToBase64String(imageBytes);

                        // Tạo chuỗi HTML chứa mã QR
                        string html = $"<img src=\"data:image/png;base64,{base64String}\" alt=\"QR Code\" />";

                        // Hiển thị chuỗi HTML trên trang web (ví dụ: Response.Write(html) trong ASP.NET)
                        return new ResponseData<string> { IsSuccess = true, Data = html };
                    }
                }
                catch (Exception e)
                {
                    return new ResponseData<string> { IsSuccess = false, Data = e.Message };
                }
            }
            else
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Ko có id ko tạo dc" };
            }
        }
        public async Task<ResponseData<string>> QrCodeCheckOut(int idBookingDetail)
        {
            if (idBookingDetail != null)
            {
                try
                {
                    BarcodeWriter<Bitmap> barcodeWriter = new BarcodeWriter<Bitmap>();
                    barcodeWriter.Format = BarcodeFormat.QR_CODE;
                    barcodeWriter.Options = new EncodingOptions { Height = 200, Width = 200 };

                    Bitmap qrCodeImage = barcodeWriter.Write("https://localhost:7039/swagger/index.html");

                    using (MemoryStream ms = new MemoryStream())
                    {
                        qrCodeImage.Save(ms, ImageFormat.Png);
                        byte[] imageBytes = ms.ToArray();
                        string base64String = Convert.ToBase64String(imageBytes);

                        // Tạo chuỗi HTML chứa mã QR
                        string html = $"<img src=\"data:image/png;base64,{base64String}\" alt=\"QR Code\" />";

                        // Hiển thị chuỗi HTML trên trang web (ví dụ: Response.Write(html) trong ASP.NET)
                        return new ResponseData<string> { IsSuccess = true, Data = html };
                    }
                }
                catch (Exception e)
                {
                    return new ResponseData<string> { IsSuccess = false, Data = e.Message };
                }
            }
            else
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Ko có id ko tạo dc" };
            }
        }
        public async Task<ResponseData<ResponseMomo>> PaymentQr(string totalPrice)
        {
            string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
            string partnerCode = "MOMO5RGX20191128";
            string accessKey = "M8brj9K6E22vXoDB";
            string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
            string orderInfo = "Chuyển khoản thanh toán đặt lịch";
            string redirectUrl = "https://localhost:7259/ListServicesBooking";
            string ipnUrl = "https://localhost:7259/ListServicesBooking";
            string requestType = "captureWallet";

            string amount = totalPrice;
            string orderId = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();

            string extraData = "{username:Tên la gi}";

            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType
                ;


            MoMoSecurity moMoSecurity = new MoMoSecurity();
            //sign signature SHA256
            string signature = moMoSecurity.signSHA256(rawHash, serectkey);


            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }

            };
            return await PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
        }
    }
}