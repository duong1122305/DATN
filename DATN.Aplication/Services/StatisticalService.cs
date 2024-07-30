using Azure;
using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.Data.Enum;
using DATN.ViewModels.Common;
using DATN.ViewModels.Common.Location;
using DATN.ViewModels.DTOs.Statistical;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
    public class StatisticalService: IStatisticalService
	{
		private readonly IUnitOfWork _ufw;

		public StatisticalService(IUnitOfWork ufw)
		{
			_ufw = ufw;
		}
		public async Task<ResponseData<Statistical>> StatisticalIndex(int type = 1)
		{

			try
			{
				var today = DateTime.Today;
				var startDate = today;
				if (type == 2)
				{
					int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
					startDate = today.AddDays(-1 * diff).Date;

				}
				else if (type == 3)
				{
					startDate = new DateTime(today.Year, today.Month, 1);

				}
				var lstBooking = await _ufw.BookingRepository.FindAsync(p => p.BookingTime.Date >= startDate && p.BookingTime.Date <= today&& p.Status==BookingStatus.Completed&& p.IsPayment);

				if (lstBooking == null || lstBooking.Count() == 0)
				{
					return new ResponseData<Statistical>(new Statistical()
					{
						DataPiceRevenue = new double[] { 0, 0 },
						CustomerStatistical=new CustomerStatistical(),
						ProductRevenueStatistical=new List<Top3Statistical>(),
						ServiceRevenueStatistical= new List<Top3Statistical>(),
						ProductQuantityStatistical=new List<Top3Statistical>(),
						ServiceQuantityStatistical= new List<Top3Statistical>(),
					});
				}
				var bookingIds = lstBooking.Select(x => x.Id).ToList();
				var lstOrderDetails = await _ufw.OrderDetailRepository.FindAsync(p => bookingIds.Contains(p.IdBooking));
				var lstProductDetail = await _ufw.ProductDetailRepository.GetAllAsync();
				var lstProduct = await _ufw.ProductRepository.GetAllAsync();
				var lstBookingDetails = await _ufw.BookingDetailRepository.FindAsync(p => bookingIds.Contains( p.BookingId));
				var lstServiceDetails = await _ufw.ServiceDetailRepository.GetAllAsync();
				var lstServices = await _ufw.ServiceRepository.GetAllAsync();
				var lstImg = await _ufw.ImageProductRepository.GetAllAsync();


				// xem lượng khách trong khoảng thời gian
				CustomerStatistical cusStatistical = new CustomerStatistical();
				cusStatistical.Data = new List<ChartData>();
				if(type==1)
				{
					var lstShift = await _ufw.ShiftRepository.GetAllAsync();
					var maxTime = lstShift.Max(p => p.To).Hours;
					cusStatistical.Name = "Số khách trong ngày";
					var timeNow = DateTime.Now.Hour > maxTime ? maxTime : DateTime.Now.Hour;
					for (int i = 7; i <= timeNow; i++)
					{
						var bookingInHour = lstBooking.Where(p => p.BookingTime.Hour == i);
						var chartData = new ChartData();
						chartData.Label = (i + "h");
						if (bookingInHour != null && bookingInHour.Any())
						{
							chartData.Value = bookingInHour.Count();
						}
						else
							chartData.Value = 0;
						cusStatistical.Data.Add(chartData);

					}
				}
				else if(type==2) 
				{
					cusStatistical.Name = "Số khách trong tuần";


					for (var date = startDate; date <= today.Date; date = date.AddDays(1))
					{

						var bookingsInDay = lstBooking.Where(p => p.BookingTime.Date == date);
						var chartData = new ChartData();
						chartData.Label = date.ToString("dd/MM");
						if (bookingsInDay != null && bookingsInDay.Any())
						{
							chartData.Value = bookingsInDay.Count();
						}
						else
						{
							chartData.Value = 0;
						}
						cusStatistical.Data.Add(chartData);
					}
				}
				else
				{
					cusStatistical.Name = "Số khách trong tháng";
					for (var date = startDate; date <= today.Date; date = date.AddDays(1))
					{
						var bookingsInDay = lstBooking.Where(p => p.BookingTime.Date == date);
						var chartData = new ChartData();
						chartData.Label = date.ToString("dd/MM");
						if (bookingsInDay != null && bookingsInDay.Any())
						{
							chartData.Value = bookingsInDay.Count();
						}
						else
						{
							chartData.Value = 0;
						}
						cusStatistical.Data.Add(chartData);
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
								  TotalAmount = g.Sum(x => x.Quantity+1),
								  TotalRevenue = g.Sum(x =>  x.Price)
							  }).ToList();

				return new ResponseData<Statistical>() 
				{ 
					IsSuccess= true,
					Data = new Statistical() 
					{ 
						CustomerStatistical= cusStatistical,
						ProductQuantityStatistical=dataSP.OrderByDescending(p=>p.TotalAmount).Take(3).ToList(),
						ProductRevenueStatistical=dataSP.OrderByDescending(p=>p.TotalRevenue).Take(3).ToList(),
						ServiceRevenueStatistical=dataDV.OrderByDescending(p=>p.TotalRevenue).Take(3).ToList(),
						ServiceQuantityStatistical=dataDV.OrderByDescending(p=>p.TotalAmount).Take(3).ToList(),
						DataPiceRevenue= new double[] {dataDV.Sum(p=>p.TotalRevenue), dataSP.Sum(p => p.TotalRevenue) }
					}

				};

			}
			catch (Exception)
			{
				return new ResponseData<Statistical>(false, "có lỗi rồi đại vương ơi");
				
			}
		}
		public async Task<ResponseData<Statistical>> ProductStatical(DateTime formDate, DateTime toDate)
		{

			try
			{
				var today = DateTime.Today;
				var startDate = today;
				
				var lstBooking = await _ufw.BookingRepository.FindAsync(p => p.BookingTime.Date >= startDate && p.BookingTime.Date <= today && p.Status == BookingStatus.Completed && p.IsPayment);

				if (lstBooking == null || lstBooking.Count() == 0)
				{
					return new ResponseData<Statistical>(new Statistical()
					{
						DataPiceRevenue = new double[] { 0, 0 },
						CustomerStatistical = new CustomerStatistical(),
						ProductRevenueStatistical = new List<Top3Statistical>(),
						ServiceRevenueStatistical = new List<Top3Statistical>(),
						ProductQuantityStatistical = new List<Top3Statistical>(),
						ServiceQuantityStatistical = new List<Top3Statistical>(),
					});
				}
				var bookingIds = lstBooking.Select(x => x.Id).ToList();
				var lstOrderDetails = await _ufw.OrderDetailRepository.FindAsync(p => bookingIds.Contains(p.IdBooking));
				var lstProductDetail = await _ufw.ProductDetailRepository.GetAllAsync();
				var lstProduct = await _ufw.ProductRepository.GetAllAsync();
				var lstBookingDetails = await _ufw.BookingDetailRepository.FindAsync(p => bookingIds.Contains(p.BookingId));
				var lstServiceDetails = await _ufw.ServiceDetailRepository.GetAllAsync();
				var lstServices = await _ufw.ServiceRepository.GetAllAsync();
				var lstImg = await _ufw.ImageProductRepository.GetAllAsync();


				// xem lượng khách trong khoảng thời gian
				CustomerStatistical cusStatistical = new CustomerStatistical();
				cusStatistical.Data = new List<ChartData>();
			
				{
					var lstShift = await _ufw.ShiftRepository.GetAllAsync();
					var maxTime = lstShift.Max(p => p.To).Hours;
					cusStatistical.Name = "Số khách trong ngày";
					var timeNow = DateTime.Now.Hour > maxTime ? maxTime : DateTime.Now.Hour;
					for (int i = 7; i <= timeNow; i++)
					{
						var bookingInHour = lstBooking.Where(p => p.BookingTime.Hour == i);
						var chartData= new ChartData();
						chartData.Label=(i + "h");
						if (bookingInHour != null && bookingInHour.Any())
						{
							chartData.Value = bookingInHour.Count();
						}
						else
							chartData.Value = 0;
						cusStatistical.Data.Add(chartData);

					}
				}
				
				{
					cusStatistical.Name = "Số khách trong tuần";


					for (var date = startDate; date <= today.Date; date = date.AddDays(1))
					{

						var bookingsInDay = lstBooking.Where(p => p.BookingTime.Date == date);
						var chartData = new ChartData();
						chartData.Label = date.ToString("dd/MM");
						if (bookingsInDay != null && bookingsInDay.Any())
						{
							chartData.Value = bookingsInDay.Count();
						}
						else
						{
							chartData.Value = 0;
						}
					}
				}
				
				{
					cusStatistical.Name = "Số khách trong tháng";
					for (var date = startDate; date <= today.Date; date = date.AddDays(1))
					{
						var bookingsInDay = lstBooking.Where(p => p.BookingTime.Date == date);
						var chartData = new ChartData();
						chartData.Label = date.ToString("dd/MM");
						if (bookingsInDay != null && bookingsInDay.Any())
						{
							chartData.Value = bookingsInDay.Count();
						}
						else
						{
							chartData.Value = 0;
						}
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

				// TÍnh doanh thu và số số lần dịch bụ đã làm
				var dataDV = (from service in lstServices
							  join serviceDetail in lstServiceDetails on service.Id equals serviceDetail.ServiceId
							  join bookingDetail in lstBookingDetails on serviceDetail.Id equals bookingDetail.ServiceDetailId into bdGroup
							  from bookingDetail in bdGroup.DefaultIfEmpty()
							  group new { ServiceName = service.Name, Price = serviceDetail.Price, Quantity = bookingDetail?.Quantity ?? 0 }
							  by new { ServiceName = service.Name } into g
							  select new Top3Statistical
							  {
								  Name = g.Key.ServiceName,
								  TotalAmount = g.Sum(x => x.Quantity + 1),
								  TotalRevenue = g.Sum(x => (x.Quantity + 1) * x.Price)
							  }).ToList();

				return new ResponseData<Statistical>()
				{
					IsSuccess = true,
					Data = new Statistical()
					{
						CustomerStatistical = cusStatistical,
						ProductQuantityStatistical = dataSP.OrderByDescending(p => p.TotalAmount).Take(3).ToList(),
						ProductRevenueStatistical = dataSP.OrderByDescending(p => p.TotalRevenue).Take(3).ToList(),
						ServiceRevenueStatistical = dataDV.OrderByDescending(p => p.TotalRevenue).Take(3).ToList(),
						ServiceQuantityStatistical = dataDV.OrderByDescending(p => p.TotalAmount).Take(3).ToList(),
						DataPiceRevenue = new double[] { dataDV.Sum(p => p.TotalRevenue), dataSP.Sum(p => p.TotalRevenue) }
					}

				};

			}
			catch (Exception)
			{
				return new ResponseData<Statistical>(false, "có lỗi rồi đại vương ơi");

			}
		}
	}
}
