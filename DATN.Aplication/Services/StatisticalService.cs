using Azure;
using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.Data.Enum;
using DATN.Utilites;
using DATN.ViewModels.Common;
using DATN.ViewModels.Common.Location;
using DATN.ViewModels.DTOs.Booking;
using DATN.ViewModels.DTOs.Statistical;
using DATN.ViewModels.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DATN.Aplication.Services
{
    public class StatisticalService : IStatisticalService
    {
        private readonly IUnitOfWork _ufw;
        private readonly UserManager<User> _userManager;

        public StatisticalService(IUnitOfWork ufw, UserManager<User> userManager)
        {
            _ufw = ufw;
            _userManager = userManager;
        }
        public async Task<ResponseData<Statistical>> StatisticalIndex(DateTime? startDate, DateTime? endDate, int type = 1)
        {

            try
            {
                var today = DateTime.Today;
                if (type == 1)
                {
                    startDate = today;
                    endDate = today;
                }
                else if (type == 2)
                {
                    int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                    startDate = today.AddDays(-1 * diff).Date;
                    endDate = today;
                }
                else if (type == 3)
                {
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = today;
                }
                var lstHistory = await _ufw.HistoryActionRepository.FindAsync(p => p.ActionID == 16 && p.ActionTime.Date >= startDate && p.ActionTime.Date <= endDate);
                var bookingIds = lstHistory.Select(x => x.BookingID).ToList();
                var lstBooking = await _ufw.BookingRepository.FindAsync(p => bookingIds.Contains(p.Id));
                var lstOrderDetails = await _ufw.OrderDetailRepository.FindAsync(p => bookingIds.Contains(p.IdBooking));
                var lstProductDetail = await _ufw.ProductDetailRepository.GetAllAsync();
                var lstProduct = await _ufw.ProductRepository.GetAllAsync();
                var lstBookingDetails = await _ufw.BookingDetailRepository.FindAsync(p => bookingIds.Contains(p.BookingId));
                var lstServiceDetails = await _ufw.ServiceDetailRepository.GetAllAsync();
                var lstServices = await _ufw.ServiceRepository.GetAllAsync();
                var lstImg = await _ufw.ImageProductRepository.GetAllAsync();
                var lstBrand = await _ufw.BrandRepository.GetAllAsync();
                var lstCate = await _ufw.CategoryDetailRepository.GetAllAsync();

                var productOutStocks = (from pd in lstProductDetail
                                        join p in lstProduct on pd.IdProduct equals p.Id
                                        join c in lstCate on p.IdCategoryDeatail equals c.Id
                                        join b in lstBrand on p.IdBrand equals b.Id
                                        where p.Status && pd.Status != ViewModels.Enum.ProductDetailStatus.Deleted && pd.Amount < 10
                                        orderby pd.Amount
                                        select new ProductOutStock()
                                        {
                                            FullName = p.Name + "- " + pd.Name,
                                            Brand = b.Name,
                                            Category = c.Name,
                                            Quantity = pd.Amount.ToString("N0"),
                                            Status = pd.Amount > 0

                                        }).ToList();

                // xem lượng khách trong khoảng thời gian
                LstDataChart cusStatistical = new LstDataChart();// data lượt khách
                LstDataChart revStatistical = new LstDataChart();// data doanh thu
                cusStatistical.Data = new List<ChartData>();
                revStatistical.Data = new List<ChartData>();
                if (type == 1)
                {

                    var lstShift = await _ufw.ShiftRepository.GetAllAsync();
                    var maxTime = lstShift.Max(p => p.To).Hours;
                    cusStatistical.Name = "Số khách trong ngày";
                    revStatistical.Name = "Doanh thu trong ngày";
                    var timeNow = DateTime.Now.Hour > maxTime ? maxTime : DateTime.Now.Hour;
                    for (int i = 7; i <= timeNow; i++)
                    {
                        var bookingInHour = lstBooking.Where(p => p.HistoryActions.Any(x => x.ActionID == 16 && x.ActionTime.Hour == i));
                        var chartData = new ChartData();
                        var dataRevenue = new ChartData();
                        chartData.Label = (i + "h");
                        dataRevenue.Label = (i + "h");
                        if (bookingInHour != null && bookingInHour.Any())
                        {
                            chartData.Value = bookingInHour.Count();
                            dataRevenue.Value = bookingInHour.Sum(p => p.TotalPrice);
                        }
                        else
                        {
                            chartData.Value = 0;
                            dataRevenue.Value = 0;
                        }
                        cusStatistical.Data.Add(chartData);
                        revStatistical.Data.Add(dataRevenue);

                    }
                }
                else if (type == 2)
                {
                    cusStatistical.Name = "Số khách trong tuần";
                    revStatistical.Name = "Doanh thu trong tuần";

                    for (var date = startDate; date <= today.Date; date = date.Value.AddDays(1))
                    {

                        var bookingsInDay = lstBooking.Where(p => p.HistoryActions.Any(x => x.ActionID == 16 && x.ActionTime.Date == date));
                        var chartData = new ChartData();
                        var dataRevenue = new ChartData();
                        chartData.Label = date.Value.ToString("dd/MM");
                        dataRevenue.Label = date.Value.ToString("dd/MM");
                        if (bookingsInDay != null && bookingsInDay.Any())
                        {
                            chartData.Value = bookingsInDay.Count();
                            dataRevenue.Value = bookingsInDay.Sum(p => p.TotalPrice);
                        }
                        else
                        {
                            chartData.Value = 0;
                            dataRevenue.Value = 0;
                        }
                        cusStatistical.Data.Add(chartData);
                        revStatistical.Data.Add(dataRevenue);
                    }
                }
                else if (type == 3)
                {
                    cusStatistical.Name = "Số khách trong tháng " + DateTime.Now.Month;
                    revStatistical.Name = "Doanh thu trong tháng " + DateTime.Now.Month;
                    for (var date = startDate; date <= today.Date; date = date.Value.AddDays(1))
                    {
                        var bookingsInDay = lstBooking.Where(p => p.HistoryActions.Any(x => x.ActionID == 16 && x.ActionTime.Date == date));
                        var dataRevenue = new ChartData();
                        var chartData = new ChartData();
                        chartData.Label = date.Value.ToString("dd");
                        dataRevenue.Label = date.Value.ToString("dd");
                        if (bookingsInDay != null && bookingsInDay.Any())
                        {
                            chartData.Value = bookingsInDay.Count();
                            dataRevenue.Value = bookingsInDay.Sum(p => p.TotalPrice);
                        }
                        else
                        {
                            chartData.Value = 0;
                            dataRevenue.Value = 0;
                        }
                        cusStatistical.Data.Add(chartData);
                        revStatistical.Data.Add(dataRevenue);
                    }
                }
                else if (type == 5)
                {
                    cusStatistical.Name = $"Số khách từ {startDate.Value.ToString("dd/MM/yyyy")} đến {endDate.Value.ToString("dd/MM/yyyy")} ";
                    revStatistical.Name = $"Doanh thu  từ {startDate.Value.ToString("dd/MM/yyyy")} đến {endDate.Value.ToString("dd/MM/yyyy")} ";
                    for (var date = startDate; date <= endDate; date = date.Value.AddDays(1))
                    {
                        var bookingsInDay = lstBooking.Where(p => p.HistoryActions.Any(x => x.ActionID == 16 && x.ActionTime.Date == date));
                        var dataRevenue = new ChartData();
                        var chartData = new ChartData();

                        chartData.Label = date.Value.ToString("dd");
                        dataRevenue.Label = date.Value.ToString("dd");
                        if (bookingsInDay != null && bookingsInDay.Any())
                        {
                            chartData.Value = bookingsInDay.Count();
                            dataRevenue.Value = bookingsInDay.Sum(p => p.TotalPrice);
                        }
                        else
                        {
                            chartData.Value = 0;
                            dataRevenue.Value = 0;
                        }
                        cusStatistical.Data.Add(chartData);
                        revStatistical.Data.Add(dataRevenue);
                    }
                }


                // TÍnh doanh thu và số số lượng sản phẩm bán ra
                var dataSP = (from product in lstProduct
                              join productDetail in lstProductDetail on product.Id equals productDetail.IdProduct into pdGroup
                              from productDetail in pdGroup.DefaultIfEmpty()
                              join orderDetail in lstOrderDetails on productDetail.Id equals orderDetail.IdProductDetail into odGroup
                              from orderDetail in odGroup.DefaultIfEmpty()
                              group new { ProductName = product.Name, ProductDetailName = productDetail.Name, Price = orderDetail != null ? orderDetail.Price : 0, Quantity = orderDetail != null ? orderDetail.Quantity : 0, ImgUrl = product.ImageProducts.FirstOrDefault().UrlImage }
                              by new { ProductName = product.Name, ProductDetailName = productDetail.Name, ImgUrl = product.ImageProducts.FirstOrDefault().UrlImage } into g
                              select new Top3Statistical
                              {
                                  Name = g.Key.ProductName + " - " + g.Key.ProductDetailName,
                                  TotalAmount = g.Sum(x => x.Quantity),
                                  TotalRevenue = g.Sum(x => x.Quantity * x.Price),
                                  ImgUrl = g.Key.ImgUrl
                              }).ToList();

                // TÍnh doanh thu và số số lần dịch vụ đã làm
                var dataDV = (from service in lstServices
                              join serviceDetail in lstServiceDetails on service.Id equals serviceDetail.ServiceId
                              join bookingDetail in lstBookingDetails on serviceDetail.Id equals bookingDetail.ServiceDetailId into bdGroup
                              from bookingDetail in bdGroup.DefaultIfEmpty()
                              group new { ServiceName = service.Name, Price = serviceDetail.Price, Quantity = bookingDetail?.Quantity ?? 0 }

                              by new { ServiceName = service.Name } into g
                              select new Top3Statistical
                              {
                                  Name = g.Key.ServiceName,
                                  TotalAmount = g.Sum(x => x.Quantity),
                                  TotalRevenue = g.Sum(x => x.Quantity * x.Price)
                              }).ToList();
                //tính tổng doanh thu dịchvụ/ sản phẩm

                LstDataChart pieData = new LstDataChart();
                var pieDV = new ChartData()
                {
                    Label = "Dịch vụ",
                    Value = dataDV.Sum(p => p.TotalRevenue),
                    Text = $"Dich vụ : {dataDV.Sum(p => p.TotalRevenue).ToString("N0")}đ"
                };
                pieData.Data.Add(pieDV);
                var pieSP = new ChartData()
                {
                    Label = "Sản phẩm",
                    Value = dataSP.Sum(p => p.TotalRevenue),
                    Text = $"Sản phẩm : {dataSP.Sum(p => p.TotalRevenue).ToString("N0")}đ"
                };
                pieData.Data.Add(pieSP);
                pieData.Name = "Tổng doanh thu: " + (dataSP.Sum(p => p.TotalRevenue) + dataDV.Sum(p => p.TotalRevenue)).ToString("N0") + "đ";
                ///

                return new ResponseData<Statistical>()
                {
                    IsSuccess = true,
                    Data = new Statistical()
                    {
                        CustomerStatistical = cusStatistical,
                        ProductQuantityStatistical = dataSP.OrderByDescending(p => p.TotalAmount).ThenByDescending(p => p.TotalRevenue).Take(3).ToList(),
                        //ProductRevenueStatistical=dataSP.OrderByDescending(p=>p.TotalRevenue).Take(3).ToList(),
                        //ServiceRevenueStatistical=dataDV.OrderByDescending(p=>p.TotalRevenue).Take(3).ToList(),
                        ServiceQuantityStatistical = dataDV.OrderByDescending(p => p.TotalAmount).Take(3).ToList(),
                        DataPiceRevenue = pieData,
                        RevenueStatistical = revStatistical,
                        LstProductOutStock = productOutStocks,

                    }

                };

            }
            catch (Exception)
            {
                return new ResponseData<Statistical>(false, "có lỗi rồi đại vương ơi");

            }
        }
        public async Task<ResponseData<List<StatiscalBill>>> StatisticalBill(DateTime? startDate, DateTime? endDate, int type = 3)
        {

            try
            {
                var today = DateTime.Today;
                if (type == 1)
                {
                    startDate = today;
                    endDate = today;
                }
                else if (type == 2)
                {
                    int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                    startDate = today.AddDays(-1 * diff).Date;
                    endDate = today;
                }
                else if (type == 3)
                {
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = today;
                }
                var lstHistory = await _ufw.HistoryActionRepository.FindAsync(p => p.ActionID == Contant.Payment && p.ActionTime.Date >= startDate && p.ActionTime.Date <= endDate);
                var bookingIds = lstHistory.Select(x => x.BookingID).ToList();
                var lstBooking = await _ufw.BookingRepository.FindAsync(p => bookingIds.Contains(p.Id));
                var lstGuest = await _ufw.GuestRepository.GetAllAsync();
                var lstUser = _userManager.Users.ToList();
                CultureInfo viCulture = new CultureInfo("vi-VN");
                var data = from h in lstHistory
                           join b in lstBooking on h.BookingID equals b.Id
                           join u in lstUser on h.ActionByID equals u.Id
                           join g in lstGuest on b.GuestId equals g.Id
                           orderby h.ActionTime descending
                           select new StatiscalBill()
                           {
                               Amount = b.TotalPrice.ToString("N0"),
                               CusName = g.Name,
                               ID = b.Id,
                               StaffName = u.FullName,
                               Status = StatusBookingToString(b.Status),
                               TimeComplete = h.ActionTime.ToString("dddd 'Ngày' dd/MM/yyyy", viCulture)
                           };

                return new ResponseData<List<StatiscalBill>>(data.ToList());
            }
            catch (Exception ex)
            {

                return new ResponseData<List<StatiscalBill>>(false, "Có lỗi xảy ra ngoài ý muốn");
            }
        }
        public async Task<ResponseData<BookingDataStatiscal>> GetDataBooking(int bookingID)
        {
            try
            {
                var lstOrder = await _ufw.OrderDetailRepository.FindAsync(p => p.IdBooking == bookingID);
                var lstBookingDetail = await _ufw.BookingDetailRepository.FindAsync(p => p.BookingId == bookingID);
                var lstService = await _ufw.ServiceRepository.GetAllAsync();
                var lstServiceDetail = await _ufw.ServiceDetailRepository.GetAllAsync();
                var lstProduct = await _ufw.ProductRepository.GetAllAsync();
                var lstProductDetail = await _ufw.ProductDetailRepository.GetAllAsync();
                var lstUser = _userManager.Users.ToList();
                var result = new BookingDataStatiscal();
                var dataBD = from b in lstBookingDetail
                             join sd in lstServiceDetail on b.ServiceDetailId equals sd.Id
                             join s in lstService on sd.ServiceId equals s.Id
                             join u in lstUser on b.StaffId equals u.Id
                             select new StatiscalBookingDetail()
                             {
                                 ServiceName = s.Name,
                                 Amount = b.Price.ToString("M0"),
                                 EndTime = b.EndDateTime.ToString("dd/MM/yyyy HH:mm"),
                                 StartTime = b.StartDateTime.ToString("dd/MM/yyyy HH:mm"),
                                 StaffName = u.FullName,
                                 Status = StatusBookingDetailToString(b.Status),
                                 ID = b.Id,

                             };
                var dataOD = from od in lstOrder
                             join pd in lstProductDetail on od.IdProductDetail equals pd.Id
                             join p in lstProduct on pd.IdProduct equals p.Id
                             select new OderStatiscal()
                             {

                                 ID = od.Id,
                                 Quantity = od.Quantity.ToString("N0"),
                                 Price = od.Price.ToString("N0"),
                                 ProductName = p.Name + "- " + pd.Name,
                                 Amount = (od.Price * od.Quantity).ToString("N0")
                             };
                var x = dataBD == null ? new List<StatiscalBookingDetail>() : dataBD.ToList();
                result.BookingDetailData = dataBD == null ? new List<StatiscalBookingDetail>() : dataBD.ToList();
                result.OderData = dataOD == null ? new List<OderStatiscal>() : dataOD.ToList();
                return new ResponseData<BookingDataStatiscal>(result);
            }
            catch (Exception ex)
            {

                return new ResponseData<BookingDataStatiscal>(false, "Có lỗi xảy ra ngoài ý muốn");
            }
        }
        public async Task<ResponseData<List<HistoryBookingVM>>> GetHistoryBooking(int bookingID)
        {
            try
            {
                var lstHistoryById = await _ufw.HistoryActionRepository.FindAsync(p => p.BookingID == bookingID);
                var lstAction = await _ufw.ActionBookingRepository.GetAllAsync();
                var lstUser = _userManager.Users.ToList();
                var result = from h in lstHistoryById
                             join a in lstAction on h.ActionID equals a.ID
                             join u in lstUser on h.ActionByID equals u.Id into uGroup
                             from u in uGroup.DefaultIfEmpty()
                             select new HistoryBookingVM()
                             {
                                 ID = h.ID,
                                 ActionBy = u != null ? u.FullName : "Khách hàng",
                                 ActionName = a.Name,
                                 TimeAction = h.ActionTime.ToString("dd/MM/yyyy HH:mm"),
                                 Description = h.Description
                             };

                return new ResponseData<List<HistoryBookingVM>>(result.ToList());
            }
            catch (Exception ex)
            {

                return new ResponseData<List<HistoryBookingVM>>(false, "Có lỗi xảy ra ngoài ý muốn");
            }
        }
        private string StatusBookingToString(BookingStatus status)
        {
            switch (status)
            {
                case BookingStatus.AdminCancelled:
                    return "Hủy bởi admin";
                case BookingStatus.StaffCancelled:
                    return "Hủy bởi nhân viên";
                case BookingStatus.CustomerCancelled:
                    return "Hủy bởi khách hàng";
                case BookingStatus.Confirmed:
                    return "Đơn hàng được xác nhận";
                case BookingStatus.PendingConfirmation:
                    return "Đơn hàng chờ xác nhận";
                case BookingStatus.InProgress:
                    return "Đơn hàng đang thực hiện";
                case BookingStatus.Arrived:
                    return "Khách hàng đang đợi";
                case BookingStatus.NoShow:
                    return "Khách hàng không đến";
                default:
                    return "Đơn hàng hoàn thành";
            }
        }
        private string StatusBookingDetailToString(BookingDetailStatus status)
        {
            switch (status)
            {
                case BookingDetailStatus.Unfulfilled:
                    return "Chưa thực hiện";
                case BookingDetailStatus.Processing:
                    return "Đang thực hiện";
                case BookingDetailStatus.Cancelled:
                    return "Đã hủy";
                default:
                    return "Hoàn thành";
            }
        }
    }
}
