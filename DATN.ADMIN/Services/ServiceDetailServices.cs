﻿using DATN.ADMIN.IServices;
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

        public async Task<ResponseData<List<ServiceDetail>>> GetServiceDetailByServiceId(int id)
        {
            var response = await _client.GetAsync($"api/ServicesDetail/getServiceDetailByServiceId/{id}");
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<ServiceDetail>>>(result);

            return data;
        }

        public async Task<List<GetServiceNameVM>> GetServiceName()
        {
            var response = await _client.GetAsync("api/ServicesDetail/getServiceName");
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<GetServiceNameVM>>(result);
            return data;
        }

        public async Task<ResponseData<string>> Remove(int id)
        {
            var response = await _client.PatchAsJsonAsync($"api/ServicesDetail/removeServiceDetail/{id}", id);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(result);

            return data;
        }

        public async Task<ResponseData<string>> Update(int id, UpdateServiceDetailVM serviceDetail)
        {
            var response = await _client.PutAsJsonAsync($"api/ServicesDetail/updateServiceDetail/{id}", serviceDetail);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(result);

            return data;
        }
    }
}
