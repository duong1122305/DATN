using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Report;
using System.Net.Http;
using System.Net.Http.Headers;

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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Key"));
        }
        public async Task<ResponseData<string>> DeleteReport(int id)
        {
            return _httpClient.GetFromJsonAsync<ResponseData<string>>($"/api/Report/delete-report?id={id}").GetAwaiter().GetResult();
        }


        public async Task<ResponseData<List<ReportVM>>> GetAllReports()
        {
            return _httpClient.GetFromJsonAsync<ResponseData<List<ReportVM>>>("/api/Report/get-all-report").GetAwaiter().GetResult();
        }

        public async Task<ResponseData<List<ReportVM>>> GetReportByGuest(Guid id)
        {
            return _httpClient.GetFromJsonAsync<ResponseData<List<ReportVM>>>($"/api/PetManager/get-report-by-id?id={id}").GetAwaiter().GetResult();
        }
    }


}
