using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Category;
using DATN.ViewModels.DTOs.CategoryProduct;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DATN.ADMIN.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CategoryServices(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = client;
            _httpContextAccessor = httpContextAccessor;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Key"));

        }
        public async Task<ResponseData<string>> ActiveCategory(int id)
        {
            var respone = await _httpClient.GetFromJsonAsync<ResponseData<string>>($"api/Cate/Active-Category?id={id}");
            return respone;
        }

        public async Task<ResponseData<string>> ActiveCategoryProduct(int id)
        {
            var respone = await _httpClient.GetFromJsonAsync<ResponseData<string>>($"api/cate/Active-Category-Product?id={id}");
            return respone;
        }

        public async Task<ResponseData<string>> CreateCategory(CategoryView categoryView)
        {
            var respone = await _httpClient.PostAsJsonAsync("api/cate/Create-Category", categoryView);
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(await respone.Content.ReadAsStringAsync());
            return data;
        }

        public async Task<ResponseData<string>> CreateCategoryProduct(CreateCategoryProductView categoryView)
        {
            var respone = await _httpClient.PostAsJsonAsync("api/cate/Create-Category-Product", categoryView);
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(await respone.Content.ReadAsStringAsync());
            return data;
        }

        public async Task<ResponseData<List<CategoryView>>> ListCategory()
        {
            var respone = await _httpClient.GetFromJsonAsync<ResponseData<List<CategoryView>>>("api/cate/List-Category");
            return respone;
        }

        public async Task<ResponseData<List<CategoryProductView>>> ListCategoryProduct()
        {

            var respone = await _httpClient.GetFromJsonAsync<ResponseData<List<CategoryProductView>>>("api/cate/List-Category-Product");
            return respone;
        }

        public async Task<ResponseData<string>> RemoveCategory(int id)
        {
            var respone = await _httpClient.GetFromJsonAsync<ResponseData<string>>($"api/cate/Remove-Category?id={id}");
            return respone;
        }

        public async Task<ResponseData<string>> RemoveCategoryProduct(int id)
        {
            var respone = await _httpClient.GetFromJsonAsync<ResponseData<string>>($"api/cate/Remove-Category-Product?id={id}");
            return respone;
        }

        public async Task<ResponseData<string>> UpdateCategory(CategoryView categoryView)
        {
            var respone = await _httpClient.PatchAsJsonAsync("api/cate/Update-Category", categoryView);
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(await respone.Content.ReadAsStringAsync());
            return data;
        }

        public async Task<ResponseData<string>> UpdateCategoryProduct(CreateCategoryProductView categoryView)
        {
            var respone = await _httpClient.PatchAsJsonAsync("api/cate/Update-Category-Product", categoryView);
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(await respone.Content.ReadAsStringAsync());
            return data;
        }
    }
}
