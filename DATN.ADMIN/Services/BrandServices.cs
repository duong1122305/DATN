using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Brand;
using Newtonsoft.Json;

namespace DATN.ADMIN.Services
{
    public class BrandServices : IBrandServices
    {
        private readonly HttpClient _httpClient;
        public BrandServices(HttpClient client)
        {
            _httpClient = client;
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
