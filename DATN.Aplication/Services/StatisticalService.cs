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
		public async Task<ResponseData<Statistical>> StatisticalIndex(DateTime? startDate, DateTime? endDate, int type = 1)
		{

			try
			{
				var today = DateTime.Today;
				if (type==1)
				{
					startDate = today;
					endDate = today;
				}
				else if (type == 2)
				{
					int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
					startDate = today.AddDays(-1 * diff).Date;
					endDate =  today;
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
				var lstBookingDetails = await _ufw.BookingDetailRepository.FindAsync(p => bookingIds.Contains( p.BookingId));
				var lstServiceDetails = await _ufw.ServiceDetailRepository.GetAllAsync();
				var lstServices = await _ufw.ServiceRepository.GetAllAsync();
				var lstImg = await _ufw.ImageProductRepository.GetAllAsync();
				var lstBrand = await _ufw.BrandRepository.GetAllAsync();
				var lstCate = await _ufw.CategoryDetailRepository.GetAllAsync();

				var productOutStocks = (from pd in lstProductDetail
							join p in lstProduct on pd.IdProduct equals p.Id
							join c in lstCate on p.IdCategoryProduct equals c.Id
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
				if(type==1)
				{

					var lstShift = await _ufw.ShiftRepository.GetAllAsync();
					var maxTime = lstShift.Max(p => p.To).Hours;
					cusStatistical.Name = "Số khách trong ngày";
					revStatistical.Name = "Doanh thu trong ngày";
					var timeNow = DateTime.Now.Hour > maxTime ? maxTime : DateTime.Now.Hour;
					for (int i = 7; i <= timeNow; i++)
					{
						var bookingInHour = lstBooking.Where(p => p.BookingTime.Hour == i);
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
				else if(type==2) 
				{
					cusStatistical.Name = "Số khách trong tuần";
					revStatistical.Name = "Doanh thu trong tuần";

					for (var date = startDate; date <= today.Date; date = date.Value.AddDays(1))
					{

						var bookingsInDay = lstBooking.Where(p => p.BookingTime.Date == date);
						var chartData = new ChartData();
						var dataRevenue = new ChartData();
						chartData.Label = date.Value.ToString("dd/MM");
						dataRevenue.Label= date.Value.ToString("dd/MM");
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
				else if(type==3)
				{
					cusStatistical.Name = "Số khách trong tháng "+ DateTime.Now.Month;
					revStatistical.Name = "Doanh thu trong tháng " + DateTime.Now.Month;
					for (var date = startDate; date <= today.Date; date = date.Value.AddDays(1))
					{
						var bookingsInDay = lstBooking.Where(p => p.BookingTime.Date == date);
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
					cusStatistical.Name = $"Số khách từ {startDate.Value.ToString("dd/MM/yyyy")} đến {endDate.Value.ToString("dd/MM/yyyy")} " ;
					revStatistical.Name = $"Doanh thu  từ {startDate.Value.ToString("dd/MM/yyyy")} đến {endDate.Value.ToString("dd/MM/yyyy")} ";
					for (var date = startDate; date <= endDate; date = date.Value.AddDays(1))
					{
						var bookingsInDay = lstBooking.Where(p => p.BookingTime.Date == date);
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
							  group new { ServiceName = service.Name, Price = serviceDetail.Price, Quantity = bookingDetail?.Quantity??0 }
							  
							  by new { ServiceName = service.Name } into g
							  select new Top3Statistical
							  {
								  Name = g.Key.ServiceName,
								  TotalAmount = g.Sum(x => x.Quantity),
								  TotalRevenue = g.Sum(x => x.Quantity* x.Price)
							  }).ToList();
				//tính tổng doanh thu dịchvụ/ sản phẩm

				LstDataChart pieData = new LstDataChart();
				var pieDV = new ChartData()
				{
					Label = "Dịch vụ",
					Value = dataDV.Sum(p => p.TotalRevenue),
					Text=$"Dich vụ : {dataDV.Sum(p => p.TotalRevenue).ToString("N0")}đ"
				};
				pieData.Data.Add(pieDV);
				var pieSP = new ChartData()
				{
					Label = "Sản phẩm",
					Value = dataSP.Sum(p => p.TotalRevenue),
					Text = $"Sản phẩm : {dataSP.Sum(p => p.TotalRevenue).ToString("N0")}đ"
				};
				pieData.Data.Add(pieSP);
				pieData.Name= "Tổng doanh thu: "+ (dataSP.Sum(p => p.TotalRevenue)+ dataDV.Sum(p => p.TotalRevenue)).ToString("N0")+ "đ";
				///

				return new ResponseData<Statistical>()
				{
					IsSuccess = true,
					Data = new Statistical()
					{
						CustomerStatistical = cusStatistical,
						ProductQuantityStatistical = dataSP.OrderByDescending(p => p.TotalAmount).ThenByDescending(p=>p.TotalRevenue).Take(3).ToList(),
						//ProductRevenueStatistical=dataSP.OrderByDescending(p=>p.TotalRevenue).Take(3).ToList(),
						//ServiceRevenueStatistical=dataDV.OrderByDescending(p=>p.TotalRevenue).Take(3).ToList(),
						ServiceQuantityStatistical = dataDV.OrderByDescending(p => p.TotalAmount).Take(3).ToList(),
						DataPiceRevenue = pieData,
						RevenueStatistical=revStatistical,
						LstProductOutStock= productOutStocks,

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
