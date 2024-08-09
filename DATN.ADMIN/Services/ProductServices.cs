using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Product;
using DATN.ViewModels.DTOs.ProductDetail;
using Newtonsoft.Json;

namespace DATN.ADMIN.Services
{
    public class ProductServices : IProductServices
    {
        private readonly HttpClient _httpClient;
        public ProductServices(HttpClient client)
        {
            _httpClient = client;
        }
        public async Task<ResponseData<string>> ActiveProduct(int id)
        {
            var respone = await _httpClient.GetFromJsonAsync<ResponseData<string>>($"api/Product/Active-Product?id={id}");
            return respone;
        }

        public async Task<ResponseData<string>> ActiveProductDetails(int id)
        {
            var respone = await _httpClient.GetFromJsonAsync<ResponseData<string>>($"api/Product/Active-Product-Details?id={id}");
            return respone;
        }

        public async Task<ResponseData<string>> CreateProduct(CreateProductView productView)
        {
            var respone = await _httpClient.PostAsJsonAsync("api/Product/Create-Product", productView);
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(await respone.Content.ReadAsStringAsync());
            return data;
        }

        public async Task<ResponseData<string>> CreateProductDetails(CreateProductDetaiView productView)
        {
            var respone = await _httpClient.PostAsJsonAsync("api/Product/Create-Product-Details", productView);
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(await respone.Content.ReadAsStringAsync());
            return data;
        }

        public async Task<ResponseData<List<ProductView>>> ListProduct()
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<List<ProductView>>>("api/Product/List-Product");
        }

        public async Task<ResponseData<List<ProductDetaiView>>> ListProductDetailForProduct(int id)
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<List<ProductDetaiView>>>($"api/Product/list-product-details-by-id?id={id}");
        }

        public async Task<ResponseData<List<ProductDetaiView>>> ListProductDetails()
        {
            return await _httpClient.GetFromJsonAsync<ResponseData<List<ProductDetaiView>>>("api/Product/List-Product-Details");
        }

        public async Task<ResponseData<string>> RemoveProduct(int id)
        {
            var respone = await _httpClient.GetFromJsonAsync<ResponseData<string>>($"api/Product/Remove-Product?id={id}");
            return respone;
        }

        public async Task<ResponseData<string>> RemoveProductDetails(int id)
        {
            var respone = await _httpClient.GetFromJsonAsync<ResponseData<string>>($"api/Product/Remove-Product-Details?id={id}");
            return respone;
        }

        public async Task<ResponseData<string>> UpdateProduct(CreateProductView productView)
        {
            var respone = await _httpClient.PutAsJsonAsync("api/Product/Update-Product", productView);
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(await respone.Content.ReadAsStringAsync());
            return data;
        }

        public async Task<ResponseData<string>> UpdateProductDetails(CreateProductDetaiView productView)
        {
            var respone = await _httpClient.PutAsJsonAsync("api/Product/Update-Product-Details", productView);
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(await respone.Content.ReadAsStringAsync());
            return data;
        }
    }
}
