using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Report;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{

	public interface IReportService
	{
		Task<ResponseData<string>> CreateReport(CreateReportRequest report);
		Task<ResponseData<List<ReportVM>>> GetAllReports();
		Task<ResponseData<string>> DeleteReport(int id);
		Task<ResponseData<List<ReportVM>>> GetReportByGuest(Guid id);
	}
	public class ReportService : IReportService
	{
		private readonly IUnitOfWork _uow;
		private readonly UserManager<User> _userManager;

		public ReportService(IUnitOfWork uow, UserManager<User> userManager)
		{
			_uow = uow;
			_userManager = userManager;
		}
		public async Task<ResponseData<List<ReportVM>>> GetAllReports()
		{
			try
			{
				var query = from rp in await _uow.ReportRepository.GetAllAsync()
							join b in await _uow.BookingRepository.GetAllAsync() on rp.BookingId equals b.Id
							join g in await _uow.GuestRepository.GetAllAsync() on b.GuestId equals g.Id
							orderby rp.CreateAt descending
							select new ReportVM
							{
								ID=rp.Id,
								BillID = b.Id,
								Comment = rp.Comment,
								DateRate = rp.CreateAt.ToString("dd/MM/yyyy"),
								Rate= rp.Rate,
								NameCustomer = g.Name
							};
				return new ResponseData<List<ReportVM>>(query.ToList());
			}
			catch (Exception ex)
			{

				return new ResponseData<List<ReportVM>>(false, "Có lỗi xảy ra: " + ex);
			}
		}
		public async Task<ResponseData<string>> DeleteReport(int id)
		{
			try
			{
				var rp = await _uow.ReportRepository.GetAsync(id);
				if (rp != null)
				{
					var result = await _uow.ReportRepository.Remove(rp);
					if (result)
					{
						return new ResponseData<string>("Xóa thành công");
					}
					else
					{
						return new ResponseData<string>(false, "Xóa thất bại");
					}
				}
				return new ResponseData<string>(false, "Report đã được xóa trước đó");
			}
			catch (Exception ex)
			{

				return new ResponseData<string>(false, "Có lỗi xảy ra: " + ex);
			}
		}
		public async Task<ResponseData<string>> CreateReport(CreateReportRequest report)
		{
			try
			{
				var bookingDetail = _uow.ReportRepository.FindAsync(p => p.BookingId == report.IdBooking);
				if (bookingDetail != null)
				{
                    return new ResponseData<string>(false, "Một booking chỉ được đánh giá 1 lần ");
                }
				var newRP = new Report()
				{
					BookingId = report.IdBooking,
					Comment = report.Comment,
					CreateAt = DateTime.Now,
					Rate = report.Rate,
				};
				await _uow.ReportRepository.AddAsync(newRP);
				await _uow.SaveChangeAsync();
				return new ResponseData<string>("Thêm thành công");
			}
			catch (Exception ex)
			{

				return new ResponseData<string>(false, "Có lỗi xảy ra: " + ex);
			}
		}
		public async Task<ResponseData<List<ReportVM>>> GetReportByGuest(Guid id)
		{
			try
			{
				var query = from rp in await _uow.ReportRepository.GetAllAsync()
							join bd in await _uow.BookingDetailRepository.GetAllAsync() on rp.BookingId equals bd.Id
							join b in await _uow.BookingRepository.GetAllAsync() on bd.BookingId equals b.Id
							join g in await _uow.GuestRepository.GetAllAsync() on b.GuestId equals g.Id
							join u in await _userManager.Users.ToListAsync() on bd.StaffId equals u.Id
							join sd in await _uow.ServiceDetailRepository.GetAllAsync() on bd.ServiceDetailId equals sd.Id
							where u.Id == id
                            orderby rp.CreateAt descending
                            select new ReportVM
							{
								ID = rp.Id,
								BillID = b.Id,
								Comment = rp.Comment,
								DateRate = rp.CreateAt.ToString("dd/MM/yyyy"),
								NameCustomer = g.Name,
								Rate = rp.Rate,
							};
				return new ResponseData<List<ReportVM>>(query.ToList());
			}
			catch (Exception ex)
			{

				return new ResponseData<List<ReportVM>>(false, "Có lỗi xảy ra: " + ex);
			}
		}
	}

}
