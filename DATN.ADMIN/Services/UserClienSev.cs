using Azure;
using DATN.ADMIN.IServices;
using DATN.ADMIN.Pages;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace DATN.ADMIN.Services
{
    public class UserClienSev : IUserClientSev
    {
        private readonly HttpClient _client;
        public UserClienSev(HttpClient client)
        {
            _client = client;

        }

        public async Task<ResponseData<string>> activeUser(string id)
        {
            var respone = await _client.GetFromJsonAsync<ResponseData<string>>($"api/UserLogin/Get-id-user?username={id}");
            var repon = await _client.GetFromJsonAsync<ResponseData<string>>($"api/UserLogin/Active-user?id={respone.Data}");
            return repon;
        }

        public async Task<ResponseData<List<UserInfView>>> GetAll()
        {
            var repon = await _client.GetFromJsonAsync<ResponseData<List<UserInfView>>>("api/UserLogin/List-User");
            return repon;
        }

        public async Task<ResponseData<string>> GetById(string id)
        {
          var respone =  await _client.GetFromJsonAsync<ResponseData<string>>($"api/UserLogin/Get-id-user?username={id}");
            var result = await _client.GetFromJsonAsync<ResponseData<string>>($"api/UserLogin/remove?id={respone.Data}");
          return result;
        }

        public async Task<ResponseData<string>> Login(UserLoginView user)
        {
            var response = await _client.PostAsJsonAsync("api/UserLogin/User-Login", user);
            var result = await response.Content.ReadFromJsonAsync<ResponseData<string>>();
            return result;
        }

        public async Task<ResponseData<string>> Register(UserRegisterView userRegisterView)
        {
            var respone= await _client.PostAsJsonAsync("api/UserLogin/User-Register", userRegisterView);
            var result = await respone.Content.ReadFromJsonAsync<ResponseData<string>>();
            return result;
        }

        public async Task<ResponseData<string>> UpdateUser(UserUpdateView userInfView,string id)
        {
            var respone2 = await _client.GetFromJsonAsync<ResponseData<string>>($"api/UserLogin/Get-id-user?username={id}");
            var respone =  await _client.PutAsJsonAsync($"api/UserLogin/Update-inf?id={respone2.Data}", userInfView);
            var result = await respone.Content.ReadFromJsonAsync<ResponseData<string>>();
            return result;
        }
    }
}
