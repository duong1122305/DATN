﻿using Azure;
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

        public async Task<ResponseData<string>> AddShuduleStaffMany(List<string> lstStaff, int idShift)
        {
            var lst = await _client.PostAsJsonAsync<List<string>>($"api/UserLogin/them-ca-one-staff?shift={idShift}", lstStaff);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await lst.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<List<UserInfView>>> GetAll()
        {
            var repon = await _client.GetFromJsonAsync<ResponseData<List<UserInfView>>>("api/UserLogin/List-User");
            return repon;
        }

        public async Task<ResponseData<List<ScheduleView>>> GetAllCaNhanVien()
        {
            return await _client.GetFromJsonAsync<ResponseData<List<ScheduleView>>>("api/UserLogin/get-all-ca-lam");
        }

        public async Task<ResponseData<string>> GetById(string id)
        {
            return await _client.GetFromJsonAsync<ResponseData<string>>($"api/UserLogin/Get-id-user?username={id}");
        }

        public async Task<ResponseData<string>> GetByIdRemove(string id)
        {
            var respone = await _client.GetFromJsonAsync<ResponseData<string>>($"api/UserLogin/Get-id-user?username={id}");
            var result = await _client.GetFromJsonAsync<ResponseData<string>>($"api/UserLogin/remove?id={respone.Data}");
            return result;
        }

        public async Task<ResponseData<string>> Login(UserLoginView user)
        {
            var response = await _client.PostAsJsonAsync("api/UserLogin/User-Login", user);
            var result = await response.Content.ReadFromJsonAsync<ResponseData<string>>();
            return result;
        }

        public async Task<ResponseData<UserInfView>> GetInfoUser(string id)
        {
            var response = await _client.GetFromJsonAsync<ResponseData<UserInfView>>($"api/UserLogin/Get-user-inf-by-token/{id}");
            return response;
        }

        //public async Task<UserInfView> statusUser(DeleteRequest<Guid> deleteRequest)
        public async Task<ResponseData<string>> Register(UserRegisterView userRegisterView)
        {
            var respone = await _client.PostAsJsonAsync("api/UserLogin/User-Register", userRegisterView);
            var result = await respone.Content.ReadFromJsonAsync<ResponseData<string>>();
            return result;
        }

        public Task<ResponseData<string>> UpdateCaNhanVien(List<string> lstStaff, int idShift)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<string>> UpdateUser(UserUpdateView userInfView, string id)
        {
            var respone2 = await _client.GetFromJsonAsync<ResponseData<string>>($"api/UserLogin/Get-id-user?username={id}");
            var respone = await _client.PutAsJsonAsync($"api/UserLogin/Update-inf?id={respone2.Data}", userInfView);
            var result = await respone.Content.ReadFromJsonAsync<ResponseData<string>>();
            return result;
        }

        public async Task<ResponseData<UserChangePasswordView>> ChangePassword(UserChangePasswordView userChangePasswordView)
        {
            var response = await _client.PutAsJsonAsync("api/UserLogin/changePassword", userChangePasswordView);
            return await response.Content.ReadFromJsonAsync<ResponseData<UserChangePasswordView>>();
        }

        public async Task<ResponseMail> ForgotPassword(string mail)
        {
            var respone = _client.GetFromJsonAsync<ResponseMail>($"api/UserLogin/User-ForgotPass?mail={mail}").GetAwaiter().GetResult();
            return respone;
        }
        public async Task<ResponseData<string>> CheckCodeOtp(string username, string code)
        {
            var result = _client.GetFromJsonAsync<ResponseData<string>>($"api/UserLogin/Check-Otp?username={username}&code={code}").GetAwaiter().GetResult();
            return result;
        }
        public async Task<ResponseData<string>> ResetPass(UserResetPassView userResetPassView)
        {
            var response = _client.PostAsJsonAsync("api/UserLogin/Reset-Pass", userResetPassView).GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await response.Content.ReadAsStringAsync());
            if (result == null)
            {
                return new ResponseData<string> { IsSuccess = false };
            }
            else
                return result;
        }
    }
}
