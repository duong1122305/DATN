using Azure;
using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using System.Collections.Generic;
using System.Net.Http;

namespace DATN.ADMIN.Services
{
    public class UserClienSev : IUserClientSev
    {
        private readonly HttpClient _client;
        public UserClienSev(HttpClient client)
        {
            _client = client;

        }
        public async Task<ResponseData<List<UserInfView>>> GetAll()
        {
            var repon = await _client.GetFromJsonAsync<ResponseData<List<UserInfView>>>("api/UserLogin/List-User");
            return repon;
        }

        public async Task<ResponseData<UserInfView>> GetById(Guid id)
        {
            return await _client.GetFromJsonAsync<ResponseData<UserInfView>>($"api/UserLogin/{id}");
        }

        public async Task<UserInfView> statusUser(DeleteRequest<Guid> deleteRequest)
        {
            var response = await _client.PutAsJsonAsync("api/UserLogin", deleteRequest);
            return await response.Content.ReadFromJsonAsync<UserInfView>();
        }

        public async Task<UserInfView> UpdateUser(UserInfView userInfView)
        {
            var response = await _client.PutAsJsonAsync("api/UserLogin", userInfView);
            return await response.Content.ReadFromJsonAsync<UserInfView>();
        }
    }
}

