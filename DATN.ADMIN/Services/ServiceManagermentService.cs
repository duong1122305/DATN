using DATN.ADMIN.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ServiceManager;
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
        public async Task<ResponseData<string>> Create(CreateServiceVM service)
        {
            var response = await _client.PostAsJsonAsync("api/Services/createService", service);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(result);

            return data;
        }

        public async Task<ResponseData<List<Service>>> GetAll()
        {
            var response = await _client.GetAsync("api/Services/getAllService");
            var result = await response.Content.ReadAsStringAsync();
            if (result == null) return new ResponseData<List<Service>>();
            else
            {
                var data = JsonConvert.DeserializeObject<ResponseData<List<Service>>>(result);
                if (data == null) return new ResponseData<List<Service>>();

                return data;
            }
        }

        public async Task<ResponseData<Service>> GetById(int id)
        {
            var response = await _client.GetAsync($"api/Services/getServiceById/{id}");
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<Service>>(result);

            return data;
        }

        public async Task<ResponseData<string>> Remove(int id)
        {
            var response = await _client.PatchAsJsonAsync($"api/Services/removeService/{id}", id);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(result);

            return data;
        }

        public async Task<ResponseData<string>> Update(int id, UpdateServiceVM service)
        {
            var response = await _client.PatchAsJsonAsync($"api/Services/updateService/{id}", service);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(result);

            return data;
        }
    }
}
