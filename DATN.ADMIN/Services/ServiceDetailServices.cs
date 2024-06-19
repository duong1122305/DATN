using DATN.ADMIN.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using Newtonsoft.Json;

namespace DATN.ADMIN.Services
{
    public class ServiceDetailServices : IServiceDetailServices
    {
        private readonly HttpClient _client;

        public ServiceDetailServices(HttpClient client)
        {
            _client = client;
        }
        public async Task<ResponseData<string>> Create(ServiceDetail serviceDetail)
        {
            var response = await _client.PostAsJsonAsync("api/ServicesDetail/createServiceDetail", serviceDetail);
            var result =await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(result);

            return data;
        }

        public async Task<ResponseData<List<ServiceDetail>>> GetAll()
        {
            var response = await _client.GetAsync("api/ServicesDetail/getAllServiceDetail");
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<ServiceDetail>>>(result);

            return data;
        }

        public async Task<ResponseData<ServiceDetail>> GetById(int id)
        {
            var response = await _client.GetAsync($"api/ServicesDetail/getServiceDetailById/{id}");
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<ServiceDetail>>(result);

            return data;
        }

        public async Task<ResponseData<List<ServiceDetail>>> GetServiceDetailByServiceId(int id)
        {
            var response = await _client.GetAsync($"api/ServicesDetail/getServiceDetailByServiceId/{id}");
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<ServiceDetail>>>(result);

            return data;
        }

        public async Task<ResponseData<string>> Remove(int id)
        {
            var response = await _client.PatchAsJsonAsync($"api/ServicesDetail/removeServiceDetail", id);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(result);

            return data;
        }

        public async Task<ResponseData<string>> Update(ServiceDetail serviceDetail)
        {
            var response = await _client.PutAsJsonAsync("api/ServicesDetail/updateServiceDetail", serviceDetail);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(result);

            return data;
        }
    }
}
