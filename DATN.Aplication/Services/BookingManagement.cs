using Azure;
using DATN.Aplication.Services.IServices;
using DATN.Aplication.System;
using DATN.Data.Entities;
using DATN.Data.Enum;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ActionBooking;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.DTOs.Payment;
using DATN.ViewModels.DTOs.Payment.DATN.ViewModels.DTOs.Payment;
using DATN.ViewModels.DTOs.Product;
using DATN.ViewModels.DTOs.ServiceDetail;
using DATN.ViewModels.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

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
        private readonly Utils utils;
        public BookingManagement(IUnitOfWork unitOfWork, UserManager<User> userManager, NotificationHub notificationHub, IAuthenticate authenticate, IProductManagement productManagement, IEmployeeScheduleManagementService employeeScheduleManagementService, IVoucherManagementService voucherManagementService, Utils utils)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _notificationHub = notificationHub;
            _user = authenticate;
            _productManagement = productManagement;
            _employeeScheduleManagementService = employeeScheduleManagementService;
            _voucherManagementService = voucherManagementService;
            this.utils = utils;
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
                        else
                        {
                            count++;
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
            var query = await _unitOfWork.BookingRepository.CallProcedure();
            if (query.Count > 0)
            {
                return new ResponseData<List<BookingView>>() { IsSuccess = true, Data = query };
            }
            else
            {
                return new ResponseData<List<BookingView>> { IsSuccess = false, Data = new List<BookingView>(), Error = "Chưa có dữ liệu" };
            }
        }
        public async Task<ResponseData<List<ListBokingDetailInDay>>> GetListBookingDetailInDay(int id)
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
                        where booking.Id == id
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
                            BookingId = booking.Id,
                            PetId = item.PetId,
                            Price = item.Price.Value,
                            StaffId = item.StaffId,
                            EndDateTime = item.DateBooking.Date.AddHours(item.EndDateTime.Hours).AddMinutes(item.EndDateTime.Minutes),
                            StartDateTime = item.DateBooking.Date.AddHours(item.StartDateTime.Hours).AddMinutes(item.StartDateTime.Minutes),
                            Status = BookingDetailStatus.Unfulfilled,
                            ServiceDetailId = item.ServiceDetailId,
                            Quantity = 1,
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
                            Description = "Tạo mới hóa đơn tại quầy",
                            BookingID = booking.Id,
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
                            BookingId = booking.Id,
                            PetId = item.PetId,
                            Price = queryServiceDetail.FirstOrDefault(c => c.Id == item.ServiceDetailId).Price,
                            StaffId = item.StaffId,
                            EndDateTime = item.DateBooking.Date.AddHours(item.EndDateTime.Hours).AddMinutes(item.EndDateTime.Minutes),
                            StartDateTime = item.DateBooking.Date.AddHours(item.StartDateTime.Hours).AddMinutes(item.StartDateTime.Minutes),
                            Status = BookingDetailStatus.Unfulfilled,
                            ServiceDetailId = item.ServiceDetailId,
                            Quantity = 1
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
        public async Task<ResponseData<string>> CheckInArrive(int idBooking)
        {
            var query = (from booking in await _unitOfWork.BookingRepository.GetAllAsync()
                         where booking.Id == idBooking
                         select booking).FirstOrDefault();
            if (query != null)
            {
                try
                {
                    if (query.Status != BookingStatus.Confirmed)
                    {
                        return new ResponseData<string> { IsSuccess = false, Error = "Không thể bắt đầu dịch vụ trong trạng thái này" };
                    }
                    else
                    {
                        query.Status = BookingStatus.Arrived;
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
                            await _unitOfWork.BookingRepository.UpdateAsync(query);
                            await _unitOfWork.SaveChangeAsync();
                            return new ResponseData<string> { IsSuccess = true, Data = "Thành công" };
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
                                if (DateTime.Now.TimeOfDay.CompareTo(item.StartDateTime.TimeOfDay.Add(new TimeSpan(0, -30, 0))) >= 0 && DateTime.Now.TimeOfDay.CompareTo(item.StartDateTime.TimeOfDay.Add(new TimeSpan(0, 10, 0))) <= 0)
                                {
                                    var update = new BookingDetail();
                                    foreach (var item1 in listBookingInDay)
                                    {
                                        foreach (var item2 in listBookingInDay)
                                        {
                                            if (item1.Id == item2.Id)
                                            {
                                                update = item2;
                                            }
                                            else
                                            {
                                                if (item2.StartDateTime.CompareTo(item1.StartDateTime) < 0)
                                                {
                                                    update = item2;
                                                }
                                            }
                                        }
                                    }
                                    update.Status = BookingDetailStatus.Processing;
                                    await _unitOfWork.BookingDetailRepository.UpdateAsync(update);
                                    await _unitOfWork.BookingRepository.UpdateAsync(query);
                                    await _unitOfWork.SaveChangeAsync();
                                    return new ResponseData<string> { IsSuccess = true, Data = "Thành công" };
                                }
                            }
                            return new ResponseData<string> { IsSuccess = false, Error = "Chưa đến giờ mà khách đặt dịch vụ hoặc quá giờ bắt đầu dịch vụ" };
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
                    var count = 0;
                    foreach (var item in queryBooking)
                    {
                        if (item.Status == BookingDetailStatus.Processing)
                        {
                            item.Status = BookingDetailStatus.Completed;
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
            try
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
                                                   Quantity = buypro.SelectQuantityProduct,
                                                   Price = buypro.Price,
                                               }).ToList();
                            var user = await _user.GetUserByToken(payment.Token);
                            HistoryAction action = new HistoryAction()
                            {
                                ActionID = 16,
                                ActionByID = Guid.Parse(user.Data),
                                ActionTime = DateTime.Now,
                                BookingID = queryBooking.Id,
                                Description = "Đây là nhân viên thanh toán cho khách nhé :)))",
                            };
                            await _unitOfWork.BookingRepository.UpdateAsync(queryBooking);
                            await _unitOfWork.HistoryActionRepository.AddAsync(action);
                            await _unitOfWork.SaveChangeAsync();
                            await _productManagement.BuyProduct(listProduct);
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
                    if (payment.LstProducts.Count > 0)
                    {
                        var booking = new Booking()
                        {
                            GuestId = Guid.Parse("cf9fa787-b64c-462a-a3ba-08dc8d178fc0"),
                            BookingTime = DateTime.Now,
                            VoucherId = null,
                            TotalPrice = 0,
                            PaymentTypeId = 1,
                            ReducedAmount = 0,
                            Status = BookingStatus.Completed,
                            IsPayment = true,
                            IsAddToSchedule = true,
                        };
                        await _unitOfWork.BookingRepository.AddAsync(booking);
                        await _unitOfWork.SaveChangeAsync();
                        if (bill.Data.IdVoucher != null)
                        {
                            var checkVoucher = (from dis in await _unitOfWork.DiscountRepository.GetAllAsync()
                                                where dis.Id == queryBooking.VoucherId
                                                select dis).FirstOrDefault();
                            checkVoucher.AmountUsed++;
                            await _unitOfWork.DiscountRepository.UpdateAsync(checkVoucher);
                        }
                        booking.TotalPrice = bill.Data.TotalPayment;
                        booking.PaymentTypeId = payment.TypePaymenId;
                        booking.ReducedAmount = bill.Data.ReducePrice;
                        booking.IsPayment = true;
                        booking.VoucherId = bill.Data.IdVoucher;
                        var listProduct = (from buypro in payment.LstProducts
                                           select new BuyProduct
                                           {
                                               IdBooking = booking.Id,
                                               IdProductDetail = buypro.IdProductDetail,
                                               Quantity = buypro.SelectQuantityProduct,
                                               Price = buypro.Price,
                                           }).ToList();
                        var user = await _user.GetUserByToken(payment.Token);
                        HistoryAction action = new HistoryAction()
                        {
                            ActionID = 16,
                            ActionByID = Guid.Parse(user.Data),
                            ActionTime = DateTime.Now,
                            BookingID = booking.Id,
                            Description = "Đây là nhân viên thanh toán cho khách nhé :)))",
                        };
                        await _unitOfWork.BookingRepository.UpdateAsync(booking);
                        await _unitOfWork.HistoryActionRepository.AddAsync(action);
                        await _unitOfWork.SaveChangeAsync();
                        await _productManagement.BuyProduct(listProduct);
                        return new ResponseData<string>() { IsSuccess = true, Data = "Thanh toán thành công" };
                    }
                    else
                    {
                        return new ResponseData<string> { IsSuccess = false, Error = "Không thể thanh toán bill trống!!!" };
                    }
                }
            }
            catch (Exception e)
            {
                return new ResponseData<string> { IsSuccess = false, Error = e.Message };
            }
        }
        public async Task<ResponseData<Bill>> CheckBill(int? idBooking, List<ProductDetailView>? productdes)
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
                var checkQuan = (from order in await _unitOfWork.OrderDetailRepository.GetAllAsync()
                                 join product in await _unitOfWork.ProductDetailRepository.GetAllAsync()
                                 on order.IdProductDetail equals product.Id
                                 where order.IdBooking == idBooking.Value
                                 select new ProductDetailView
                                 {
                                     IdBooking = order.IdBooking,
                                     IdProductDetail = order.IdProductDetail,
                                     Name = product.Name,
                                     Price = order.Price,
                                     Quantity = order.Quantity,
                                     Status = product.Status,
                                     SelectQuantityProduct = order.Quantity,
                                 }).ToList();
                if (checkQuan.Count > 0)
                {
                    productdes = checkQuan;
                }
            }
            if (productdes != null && productdes.Count > 0)
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
                if (queryBooking.Count > 0 || idBooking.Value != null && idBooking.Value != 0)
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
                            IsPayment = (await _unitOfWork.BookingRepository.GetAllAsync()).FirstOrDefault(c => c.Id == idBooking.Value).IsPayment
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
                            GuestName = idBooking != null && idBooking != 0 ? info.Name : "Khách lẻ",
                            Address = idBooking != null && idBooking != 0 ? info.Address : "Không có",
                            PhoneNumber = idBooking != null && idBooking != 0 ? info.PhoneNumber : "Không có",
                            IsPayment = false
                        }
                    };
                }
            }
            else
                return new ResponseData<Bill> { IsSuccess = true, Data = new Bill() };
        }
        public async Task<ResponseData<string>> QrCodeCheckIn(int idBooking)
        {
            if (idBooking != 0)
            {
                try
                {
                    var barcodeWriter = new BarcodeWriter<Bitmap>()
                    {
                        Format = BarcodeFormat.QR_CODE,
                        Options = new EncodingOptions
                        {
                            Height = 200,
                            Width = 200,
                            Margin = 0,
                            // Chọn renderer để tạo ảnh màu
                            PureBarcode = false
                        }
                    };

                    // Sử dụng renderer để tạo ảnh màu
                    barcodeWriter.Renderer = new MyBitmapRenderer();

                    using (var bitmap = barcodeWriter.Write($"https://sd33.datlich.id.vn/public/{idBooking}"))
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(ms, ImageFormat.Png);
                        byte[] imageBytes = ms.ToArray();
                        string base64String = Convert.ToBase64String(imageBytes);

                        string html = $"data:image/png;base64,{base64String}";
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
            if (idBookingDetail != 0)
            {
                try
                {
                    var barcodeWriter = new BarcodeWriter<Bitmap>()
                    {
                        Format = BarcodeFormat.QR_CODE,
                        Options = new EncodingOptions
                        {
                            Height = 200,
                            Width = 200,
                            Margin = 0,
                            // Chọn renderer để tạo ảnh màu
                            PureBarcode = false
                        }
                    };

                    // Sử dụng renderer để tạo ảnh màu
                    barcodeWriter.Renderer = new MyBitmapRenderer();

                    using (var bitmap = barcodeWriter.Write("https://mewshop.datlich.id.vn/report/"))
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(ms, ImageFormat.Png);
                        byte[] imageBytes = ms.ToArray();
                        string base64String = Convert.ToBase64String(imageBytes);

                        string html = $"data:image/png;base64,{base64String}";
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
        public async Task<ResponseData<ResponseMomo>> PaymentQrMomo(int? id, Payment payment)
        {
            try
            {
                if (id == 0 || id == null)
                {
                    var booking = new Booking()
                    {
                        GuestId = payment.IdGuest == null ? Guid.Parse("cf9fa787-b64c-462a-a3ba-08dc8d178fc0") : payment.IdGuest.Value,
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
                    var bill = await CheckBill(null, payment.LstProducts);
                    if (bill.Data.IdVoucher != null)
                    {
                        var checkVoucher = (from dis in await _unitOfWork.DiscountRepository.GetAllAsync()
                                            where dis.Id == bill.Data.IdVoucher
                                            select dis).FirstOrDefault();
                        checkVoucher.AmountUsed++;
                        await _unitOfWork.DiscountRepository.UpdateAsync(checkVoucher);
                    }
                    booking.TotalPrice = bill.Data.TotalPayment;
                    booking.PaymentTypeId = payment.TypePaymenId;
                    booking.ReducedAmount = bill.Data.ReducePrice;
                    booking.IsPayment = true;
                    booking.VoucherId = bill.Data.IdVoucher;
                    var listProduct = (from buypro in payment.LstProducts
                                       select new BuyProduct
                                       {
                                           IdBooking = booking.Id,
                                           IdProductDetail = buypro.IdProductDetail,
                                           Quantity = buypro.SelectQuantityProduct,
                                           Price = buypro.Price,
                                       }).ToList();
                    var user = await _user.GetUserByToken(payment.Token);
                    HistoryAction action = new HistoryAction()
                    {
                        ActionByID = Guid.Parse(user.Data),
                        ActionID = 16,
                        ActionTime = DateTime.Now,
                        BookingID = booking.Id,
                        Description = "Khách tạo mã momo thanh toán đây là trạng thái chờ thanh toán",
                    };
                    await _unitOfWork.BookingRepository.UpdateAsync(booking);
                    await _unitOfWork.HistoryActionRepository.AddAsync(action);
                    await _unitOfWork.SaveChangeAsync();
                    await _productManagement.BuyProduct(listProduct);
                    id = booking.Id;
                }
            }
            catch (Exception e)
            {
                return new ResponseData<ResponseMomo> { IsSuccess = false, Data = new ResponseMomo { resultCode = 2003, message = e.Message } };
            }
            string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
            string partnerCode = "MOMO5RGX20191128";
            string accessKey = "M8brj9K6E22vXoDB";
            string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
            string orderInfo = "Chuyển khoản thanh toán làm dịch vụ MewShop";
            string redirectUrl = "https://localhost:7259/ListServicesBooking";
            string ipnUrl = $"https://api.datlich.id.vn/api/Booking/Check-Status/{id}";
            string requestType = "captureWallet";

            string amount = (payment.TotalPrice - payment.Reduce).ToString().Replace(" ", "").TrimStart().TrimEnd();
            string orderId = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();

            string extraData = "";

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


            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

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
        public async Task CheckStatusPayment(int id, MomoResultRequest momoResult)
        {
            if (id != null && id != 0)
            {
                var booking = (from bookingTable in await _unitOfWork.BookingRepository.GetAllAsync()
                               where bookingTable.Id == id
                               select bookingTable).FirstOrDefault();
                var bill = await CheckBill(id, null);
                booking.Status = BookingStatus.Completed;
                booking.IsPayment = true;
                booking.TotalPrice = bill.Data.TotalPrice;
                booking.ReducedAmount = bill.Data.ReducePrice;
                booking.VoucherId = bill.Data.IdVoucher;
                booking.PaymentTypeId = 2;
                booking.IsAddToSchedule = true;
                await _unitOfWork.BookingRepository.UpdateAsync(booking);
                await _unitOfWork.BookingRepository.SaveChangesAsync();
            }
        }

        public async Task<ResponseData<string>> AddService(CreateServiceDetail createBookingDetailRequest)
        {
            if (createBookingDetailRequest != null)
            {
                try
                {
                    var query = (from booking in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                                 where booking.BookingId == createBookingDetailRequest.BookingId
                                 select booking).ToList();

                    var queryServiceDetail = from detail in await _unitOfWork.ServiceDetailRepository.GetAllAsync()
                                             select detail;
                    for (global::System.Int32 i = 0; i < query.Count; i++)
                    {
                        var term = query[i];
                        if (term.ServiceDetailId == createBookingDetailRequest.ServiceDetailId && term.PetId == createBookingDetailRequest.PetId)
                        {
                            return new ResponseData<string> { IsSuccess = false, Error = "Trong các dịch vụ đã chọn có 1 dịch vụ sử dùng 2 lần cho 1 bé thú cưng!!!" };
                        }
                        else if (term.ServiceDetailId == createBookingDetailRequest.ServiceDetailId && term.StaffId == createBookingDetailRequest.StaffId)
                        {
                            if (term.StartDateTime.CompareTo(createBookingDetailRequest.StartDateTime) > 0 && term.StartDateTime.CompareTo(createBookingDetailRequest.EndDateTime) < 0)
                            {
                                return new ResponseData<string> { IsSuccess = false, Error = "Trong các dịch vụ đã chọn có dịch vụ chung 1 người làm và cùng 1 thời điểm!!!" };
                            }
                            else if (term.EndDateTime.CompareTo(createBookingDetailRequest.StartDateTime) > 0 && term.EndDateTime.CompareTo(createBookingDetailRequest.EndDateTime) < 0)
                            {
                                return new ResponseData<string> { IsSuccess = false, Error = "Trong các dịch vụ đã chọn có dịch vụ chung 1 người làm và cùng 1 thời điểm!!!" };
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    var bookingDetail = new BookingDetail()
                    {
                        BookingId = createBookingDetailRequest.BookingId,
                        PetId = createBookingDetailRequest.PetId,
                        Price = createBookingDetailRequest.Price,
                        StaffId = createBookingDetailRequest.StaffId,
                        EndDateTime = createBookingDetailRequest.DateBooking.Date.AddHours(createBookingDetailRequest.EndDateTime.Hours).AddMinutes(createBookingDetailRequest.EndDateTime.Minutes),
                        StartDateTime = createBookingDetailRequest.DateBooking.Date.AddHours(createBookingDetailRequest.StartDateTime.Hours).AddMinutes(createBookingDetailRequest.StartDateTime.Minutes),
                        Status = BookingDetailStatus.Unfulfilled,
                        ServiceDetailId = createBookingDetailRequest.ServiceDetailId,
                        Quantity = 1,
                    };
                    await _unitOfWork.BookingDetailRepository.AddAsync(bookingDetail);
                    var idUserAction = await _user.GetUserByToken(createBookingDetailRequest.Token);
                    if (idUserAction.IsSuccess)
                    {
                        var history = new HistoryAction()
                        {
                            ActionByID = Guid.Parse(idUserAction.Data),
                            ActionTime = DateTime.Now,
                            ActionID = 12,
                            Description = "Thêm dịch vụ con cho booking",
                            BookingID = createBookingDetailRequest.BookingId,
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
        public async Task<ResponseData<string>> PaymentQrVnPay(long totalPrice)
        {
            string vnp_Returnurl = "https://localhost:7259/ListServicesBooking"; //URL nhan ket qua tra ve 
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ""; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ""; //Secret Key

            //Get payment input
            OrderInfo order = new OrderInfo();
            order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            order.Amount = totalPrice; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
            order.CreatedDate = DateTime.Now;
            //Save order to db

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing
            vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"));
            //Billing

            //var fullName = "Nguyễn Đức Việt".Trim();
            //if (!String.IsNullOrEmpty(fullName))
            //{
            //    var indexof = fullName.IndexOf(' ');
            //    vnpay.AddRequestData("vnp_Bill_FirstName", fullName.Substring(0, indexof));
            //    vnpay.AddRequestData("vnp_Bill_LastName", fullName.Substring(indexof + 1,
            //    fullName.Length - indexof - 1));
            //}
            //vnpay.AddRequestData("vnp_Bill_Address", "vn".Trim());
            //vnpay.AddRequestData("vnp_Bill_City", "".Trim());
            //vnpay.AddRequestData("vnp_Bill_Country", "".Trim());
            //vnpay.AddRequestData("vnp_Bill_State", "");
            //// Invoice
            //vnpay.AddRequestData("vnp_Inv_Phone", "".Trim());
            //vnpay.AddRequestData("vnp_Inv_Email", "".Trim());
            //vnpay.AddRequestData("vnp_Inv_Customer", "".Trim());
            //vnpay.AddRequestData("vnp_Inv_Address", "".Trim());
            //vnpay.AddRequestData("vnp_Inv_Type", "");
            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return new ResponseData<string> { IsSuccess = true, Data = paymentUrl };
        }

        public async Task<ResponseData<List<GetBookingByGuestVM>>> GetBookingByGuest(Guid idGuest)
        {
            try
            {
                if (idGuest == Guid.Empty) return new ResponseData<List<GetBookingByGuestVM>>
                {
                    IsSuccess = false,
                    Data = new List<GetBookingByGuestVM>(),
                    Error = "Không xác định được danh tính của bạn"
                };

                var join = (from b in await _unitOfWork.BookingRepository.GetAllAsync()
                            join bd in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                            on b.Id equals bd.BookingId
                            join sd in await _unitOfWork.ServiceDetailRepository.GetAllAsync()
                            on bd.ServiceDetailId equals sd.Id
                            join s in await _unitOfWork.ServiceRepository.GetAllAsync()
                            on sd.ServiceId equals s.Id
                            join p in await _unitOfWork.PetRepository.GetAllAsync()
                            on bd.PetId equals p.Id
                            where b.GuestId == idGuest
                            select new
                            {
                                PetName = p.Name,
                                BookingId = b.Id,
                                ServiceId = s.Id,
                                ServiceName = s.Name,
                                BookingTime = b.BookingTime,
                                StartDate = bd.StartDateTime,
                                EndDate = bd.EndDateTime,
                                StartTime = bd.StartDateTime,
                                TotalPrice = b.TotalPrice,
                                Status = bd.Status
                            })
                           .GroupBy(c => new
                           {
                               c.BookingId,
                               c.PetName,
                               c.BookingTime,
                               c.StartDate,
                               c.StartTime
                           })
                           .Select(c => new GetBookingByGuestVM
                           {
                               PetName = c.Key.PetName,
                               ServiceName = c.Select(x => x.ServiceName).ToList(), // Danh sách tên dịch vụ
                               ServiceId = c.Select(x => x.ServiceId).ToList(),    // Danh sách ID dịch vụ
                               BookingTime = new DateOnly(c.Key.BookingTime.Year, c.Key.BookingTime.Month, c.Key.BookingTime.Day).ToString("dd/MM/yyyy"),
                               StartDate = new DateOnly(c.Key.StartDate.Year, c.Key.StartDate.Month, c.Key.StartDate.Day).ToString("dd/MM/yyyy"),
                               StartTime = new TimeOnly(c.Key.StartTime.Hour, c.Key.StartTime.Minute).ToString("HH:mm"),
                               TotalPrice = c.Sum(x => x.TotalPrice), // Tổng giá của các dịch vụ
                               Status = c.First().Status
                           }).OrderByDescending(c => c.BookingTime).AsQueryable();
                if (join == null) return new ResponseData<List<GetBookingByGuestVM>>
                {
                    IsSuccess = false,
                    Data = new List<GetBookingByGuestVM>(),
                    Error = "Có lỗi trong quá trình tìm kiếm"
                };

                return new ResponseData<List<GetBookingByGuestVM>>
                {
                    IsSuccess = true,
                    Data = join.ToList(),
                    Error = null
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<GetBookingByGuestVM>>
                {
                    IsSuccess = false,
                    Data = new List<GetBookingByGuestVM>(),
                    Error = ex.Message
                };
            }
        }
        public async Task<ResponseData<string>> CreateBookingForGuestNoAcount(BookingForGuestNoAccount booking)
        {
            var checkGuest = from guestTable in await _unitOfWork.GuestRepository.GetAllAsync()
                             where guestTable.PhoneNumber == booking.PhoneNumber
                             select guestTable;
            Guest guest;
            if (!checkGuest.Any())
            {
                guest = new Guest
                {
                    Id = Guid.NewGuid(),
                    IsComfirm = false,
                    Gender = true,
                    PhoneNumber = booking.PhoneNumber,
                    Name = booking.NameGuest,
                    Address = ""
                };
                await _unitOfWork.GuestRepository.AddAsync(guest);
            }
            else
            {
                guest = checkGuest.FirstOrDefault();
            }
            Pet pet = new Pet();
            if (booking.LstBookingDetail[0].PetId == null)
            {
                pet.Name = booking.NamePet;
                pet.Gender = booking.GenderPet;
                pet.OwnerId = guest.Id;
                pet.SpeciesId = booking.SpeciesId;
                await _unitOfWork.PetRepository.AddAsync(pet);
            }
            await _unitOfWork.SaveChangeAsync();
            if (booking.LstBookingDetail.Count > 0)
            {
                try
                {
                    var query = from serviceDetail in await _unitOfWork.ServiceDetailRepository.GetAllAsync()
                                select serviceDetail;
                    for (global::System.Int32 i = 0; i < booking.LstBookingDetail.Count; i++)
                    {
                        if (i == 0)
                        {
                            int time = (int)(query.FirstOrDefault(c => c.Id == booking.LstBookingDetail[0].ServiceDetailId).Duration) / 60;
                            booking.LstBookingDetail[0].EndDateTime = booking.LstBookingDetail[0].StartDateTime.Add(new TimeSpan(time, (int)(query.FirstOrDefault(c => c.Id == booking.LstBookingDetail[0].ServiceDetailId).Duration - (time * 60)), 0));
                        }
                        else
                        {
                            int time = (int)(query.FirstOrDefault(c => c.Id == booking.LstBookingDetail[0].ServiceDetailId).Duration) / 60;
                            booking.LstBookingDetail[i].StartDateTime = booking.LstBookingDetail[i - 1].EndDateTime;
                            booking.LstBookingDetail[i].EndDateTime = booking.LstBookingDetail[i].StartDateTime.Add(new TimeSpan(time, (int)(query.FirstOrDefault(c => c.Id == booking.LstBookingDetail[i].ServiceDetailId).Duration - (time * 60)), 0));
                        }
                    }

                    foreach (var item in booking.LstBookingDetail)
                    {
                        var lst = await _employeeScheduleManagementService.ListStaffFreeInTime(item.StartDateTime, item.EndDateTime, item.DateBooking.Date);
                        if (lst.IsSuccess)
                        {
                            if (item.PetId == null)
                            {
                                item.PetId = pet.Id;
                            }
                            item.StaffId = lst.Data.FirstOrDefault().IdStaff;
                            item.Price = (double)query.FirstOrDefault(c => c.Id == item.ServiceDetailId).Price;
                        }
                        else
                        {
                            return new ResponseData<string> { IsSuccess = false, Error = lst.Error };
                        }
                    }

                    var bookingInsert = new Booking()
                    {
                        GuestId = guest.Id,
                        BookingTime = DateTime.Now,
                        VoucherId = booking.VoucherId,
                        TotalPrice = 0,
                        PaymentTypeId = 1,
                        ReducedAmount = 0,
                        Status = BookingStatus.PendingConfirmation,
                        IsPayment = false,
                        IsAddToSchedule = false,
                    };
                    await _unitOfWork.BookingRepository.AddAsync(bookingInsert);
                    await _unitOfWork.SaveChangeAsync();

                    List<BookingDetail> list = new List<BookingDetail>();
                    var queryServiceDetail = from detail in await _unitOfWork.ServiceDetailRepository.GetAllAsync()
                                             select detail;
                    for (global::System.Int32 i = 0; i < booking.LstBookingDetail.Count; i++)
                    {
                        for (global::System.Int32 j = 0; j < booking.LstBookingDetail.Count; j++)
                        {
                            if (i == j)
                            {
                                continue;
                            }
                            else
                            {
                                var term1 = booking.LstBookingDetail[i];
                                var term2 = booking.LstBookingDetail[j];
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
                    foreach (var item in booking.LstBookingDetail)
                    {
                        var bookingDetail = new BookingDetail()
                        {
                            BookingId = bookingInsert.Id,
                            PetId = item.PetId,
                            Price = queryServiceDetail.FirstOrDefault(c => c.Id == item.ServiceDetailId).Price,
                            StaffId = item.StaffId,
                            EndDateTime = item.DateBooking.Date.AddHours(item.EndDateTime.Hours).AddMinutes(item.EndDateTime.Minutes),
                            StartDateTime = item.DateBooking.Date.AddHours(item.StartDateTime.Hours).AddMinutes(item.StartDateTime.Minutes),
                            Status = BookingDetailStatus.Unfulfilled,
                            ServiceDetailId = item.ServiceDetailId,
                            Quantity = 1
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
    }
    public class MyBitmapRenderer : IBarcodeRenderer<Bitmap>
    {
        public Bitmap Render(BitMatrix matrix, BarcodeFormat format, string content, EncodingOptions options)
        {
            var width = matrix.Width;
            var height = matrix.Height;
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (matrix[i, j])
                        {
                            g.FillRectangle(Brushes.Black, i, j, 1, 1);
                        }
                    }
                }
            }
            return bitmap;
        }

        public Bitmap Render(BitMatrix matrix, BarcodeFormat format, string content)
        {

            var width = matrix.Width;
            var height = matrix.Height;
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (matrix[i, j])
                        {
                            g.FillRectangle(Brushes.Black, i, j, 1, 1);
                        }
                    }
                }
            }
            return bitmap;
        }
    }

}