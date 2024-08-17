using DATN.ADMIN.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using System.Net.Http.Headers;

namespace DATN.ADMIN.Services
{
    public class PetManagerServices : IPetManagerServices
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PetManagerServices(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Key"));
        }
        public async Task<ResponseData<List<PetType>>> GetAllTypes()
        {
            return _httpClient.GetFromJsonAsync<ResponseData<List<PetType>>>("/api/PetManager/get-types").GetAwaiter().GetResult();
        }
    }
}
