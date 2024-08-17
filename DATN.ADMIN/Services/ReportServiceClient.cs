using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Report;
using System.Net.Http;

namespace DATN.ADMIN.Services
{
	public interface IReportClient
	{
		//Task<ResponseData<string>> CreateReport(CreateReportRequest report);
		Task<ResponseData<List<ReportVM>>> GetAllReports();
		Task<ResponseData<string>> DeleteReport(int id);
		Task<ResponseData<List<ReportVM>>> GetReportByGuest(Guid id);
	}
	public class ReportClient : IReportClient
	{
		private readonly HttpClient _httpClient;

		public ReportClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<ResponseData<string>> DeleteReport(int id)
		{
			return  _httpClient.GetFromJsonAsync<ResponseData<string>>($"/api/Report/delete-report?id={id}").GetAwaiter().GetResult();
		}


		public async Task<ResponseData<List<ReportVM>>> GetAllReports()
		{
			return  _httpClient.GetFromJsonAsync<ResponseData<List<ReportVM>>>("/api/Report/get-all-report").GetAwaiter().GetResult();
		}

		public async Task<ResponseData<List<ReportVM>>> GetReportByGuest(Guid id)
		{
			return  _httpClient.GetFromJsonAsync<ResponseData<List<ReportVM>>>($"/api/PetManager/get-report-by-id?id={id}").GetAwaiter().GetResult();
		}
	}


}
