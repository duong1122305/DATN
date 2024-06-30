using Azure;
using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Category;
using DATN.ViewModels.DTOs.CategoryProduct;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace DATN.ADMIN.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly HttpClient _httpClient;
        public CategoryServices(HttpClient client)
        {
                _httpClient = client;
        }
        public async Task<ResponseData<string>> ActiveCategory(int id)
        {
            var respone = await _httpClient.GetFromJsonAsync<ResponseData<string>>($"api/Cate/Active-Category?id={id}");
            return respone;
        }

        public async Task<ResponseData<string>> CreateCategory(CategoryView categoryView)
        {
            var respone = await _httpClient.PostAsJsonAsync("api/cate/Create-Category", categoryView);
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(await respone.Content.ReadAsStringAsync());
            return data!;
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

        public async Task<ResponseData<string>> UpdateCategory(CategoryView categoryView)
        {
            var respone = await _httpClient.PutAsJsonAsync("api/cate/Update-Category", categoryView);
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(await respone.Content.ReadAsStringAsync());
            return data;
        }
    }
}
