using DATN.ADMIN.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ServiceDetail;
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
        public async Task<ResponseData<string>> Create(CreateServiceDetailVM serviceDetail)
        {
            var response = await _client.PostAsJsonAsync("api/ServicesDetail/createServiceDetail", serviceDetail);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(result);

            return data;
        }

        public async Task<ResponseData<List<ServiceDetail>>> GetAll()
        {
            var response = await _client.GetAsync("api/ServicesDetail/getAllServicesDetail");
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

        public async Task<string> GetSerivceNameByServiceId(int id)
        {
            string name = "";
            var findServiceId = await _client.GetAsync($"api/Services/getServiceById/{id}");
            var result = await findServiceId.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<Service>>(result);
            if (data != null)
            {
                foreach (var item in GetAll().GetAwaiter().GetResult().Data)
                {
                    if (item.ServiceId == data.Data.Id)
                    {
                        name = data.Data.Name;
                        break;
                    }
                }
            }

            return name;
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
