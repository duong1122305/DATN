using DATN.ADMIN.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using Newtonsoft.Json;

namespace DATN.ADMIN.Services
{
    public class ServiceManagermentService : IServiceManagermentService
    {
        private readonly HttpClient _client;

        public ServiceManagermentService(HttpClient client)
        {
            _client = client;
        }
        public async Task<ResponseData<string>> Create(Service service)
        {
            var response = await _client.PatchAsJsonAsync("api/Services/createService", service);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(result);

            return data;
        }

        public async Task<ResponseData<List<Service>>> GetAll()
        {
            var response = await _client.GetAsync("api/Services/getAllService");
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<Service>>>(result);

            return data;
        }

        public async Task<ResponseData<Service>> GetById(int id)
        {
            var response = await _client.GetAsync($"api/Service/getServiceById/{id}");
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<Service>>(result);

            return data;
        }

        public async Task<ResponseData<string>> Remove(int id)
        {
            var response = await _client.PatchAsJsonAsync("api/Service/removeService/{id}", id);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(result);

            return data;
        }

        public async Task<ResponseData<string>> Update(Service service)
        {
            var responst = await _client.PutAsJsonAsync("api/Service/updateService", service);
            var result = await responst.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(result);

            return data;
        }
    }
}
