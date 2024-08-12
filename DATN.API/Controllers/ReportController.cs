using DATN.Aplication.Services;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReportController : ControllerBase
	{
		private readonly IReportService _sevice;

		public ReportController(IReportService sevice)
		{
			_sevice = sevice;
		}
		[HttpPost("create-report")]
		public async Task<ResponseData<string>> CreateReport(CreateReportRequest report)
		{
			return await _sevice.CreateReport(report);
		}
		[HttpGet("get-all-report")]
		public async Task<ResponseData<List<ReportVM>>> GetAllReports()
		{
			return await _sevice.GetAllReports();
		}
		[HttpGet("delete-report")]
		public async Task<ResponseData<string>> DeleteReport(int id) => await _sevice.DeleteReport(id);		
		[HttpGet("get-report-by-id")]
		public async Task<ResponseData<List<ReportVM>>> GetByID(Guid id) => await _sevice.GetReportByGuest(id);
	}
}
