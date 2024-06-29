using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.CategoryProduct;
using Newtonsoft.Json;

namespace DATN.ADMIN.Services
{
    public class ProductsServices : IProductsServices
    {
        private readonly HttpClient _httpClient;
        public ProductsServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseData<string>> ActiveCategoryProduct(int id)
        {
            var respone = await _httpClient.GetFromJsonAsync<ResponseData<string>>($"api/cate/Active-Category-Product?id={id}");
            return respone;
        }

        public async Task<ResponseData<string>> CreateCategoryProduct(CreateCategoryProductView categoryView)
        {
            var respone = await _httpClient.PostAsJsonAsync("api/cate/Create-Category-Product", categoryView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await respone.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<List<CategoryProductView>>> ListCategoryProduct()
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<List<CategoryProductView>>>("api/cate/List-Category-Product");
        }

        public async Task<ResponseData<string>> RemoveCategoryProduct(int id)
        {
            var respone = await _httpClient.GetFromJsonAsync<ResponseData<string>>($"api/cate/Remove-Category-Product?id{id}");
            return respone;
        }

        public async Task<ResponseData<string>> UpdateCategoryProduct(CreateCategoryProductView categoryView)
        {
            var respone = await _httpClient.PutAsJsonAsync("api/cate/Upadte-Category-Product", categoryView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await respone.Content.ReadAsStringAsync());
            return result;
        }
    }
}
