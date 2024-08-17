using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Brand;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DATN.ADMIN.Services
{
    public class BrandServices : IBrandServices
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BrandServices(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = client;
            _httpContextAccessor = httpContextAccessor;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Key"));

        }
        public async Task<ResponseData<string>> Active(int id)
        {
            var respone = await _httpClient.GetFromJsonAsync<ResponseData<string>>($"api/Brand/Active-Brand?id={id}");
            return respone;
        }

        public async Task<ResponseData<string>> CreateBrand(BrandView brandView)
        {
            var respone = await _httpClient.PostAsJsonAsync("api/Brand/Create-Brand", brandView);
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(await respone.Content.ReadAsStringAsync());
            return data!;
        }

        public async Task<ResponseData<List<BrandView>>> ListBrand()
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<List<BrandView>>>("api/Brand/List-Brand");

        }

        public async Task<ResponseData<string>> RemoveBrand(int id)
        {
            var respone = await _httpClient.GetFromJsonAsync<ResponseData<string>>($"api/Brand/Remove-Brand?id={id}");
            return respone;
        }

        public async Task<ResponseData<string>> UpdateBrand(BrandView brandView)
        {
            var respone = await _httpClient.PatchAsJsonAsync("api/Brand/Update-Brand", brandView);
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(await respone.Content.ReadAsStringAsync());
            return data;
        }
    }
}
