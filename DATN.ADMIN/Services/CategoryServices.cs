using Azure;
using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Category;
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
        public Task<ResponseData<string>> ActiveCategory(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<string>> CreateCategory(CategoryView categoryView)
        {
            var respone = await _httpClient.PostAsJsonAsync("api/UserLogin/Create-Category", categoryView);
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(await respone.Content.ReadAsStringAsync());
            return data!;
        }

        public async Task<ResponseData<List<CategoryView>>> ListCategory()
        {
            var respone = await _httpClient.GetFromJsonAsync<ResponseData<List<CategoryView>>>("api/UserLogin/List-Category");
            return respone;
        }

        public Task<ResponseData<string>> RemoveCategory(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<string>> UpdateCategory(CategoryView categoryView)
        {
            throw new NotImplementedException();
        }
    }
}
