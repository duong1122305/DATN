using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using DATN.ViewModels.DTOs.Guest;
using Newtonsoft.Json;

namespace DATN.ADMIN.Services
{
    public class GuestManagerClient:IGuestManagerClient
    {
        private readonly HttpClient _client;
        public GuestManagerClient(HttpClient client)
        {
            _client = client;

        }
        public async Task<ResponseData<List<GuestViewModel>>> GetGuest()
        {
            try
            {

                var reponse =  _client.GetFromJsonAsync<ResponseData<List<GuestViewModel>>>("api/GuestManager/GetGuest").GetAwaiter().GetResult();
                return reponse;
            }
            catch (Exception ex)
            {

                return new ResponseData<List<GuestViewModel>>()
                {
                    IsSuccess = false,
                    Error=$"Có lỗi khi lấy dữ liệu: {ex.Message}"
                };
            }
        }
        public async Task<ResponseData<string>> RegisterNoUser(GuestRegisterNoUserRequest request)
        {
            try
            {

                var reponse =  _client.PostAsJsonAsync("api/GuestManager/register-guest-no-user", request).GetAwaiter().GetResult();
                if (reponse.IsSuccessStatusCode)
                {
                   return   JsonConvert.DeserializeObject<ResponseData<string>>(await reponse.Content.ReadAsStringAsync());
                }
                return new ResponseData<string>()
                {
                    IsSuccess = false,
                    Error = $"Phát sinh lỗi không rõ khi gửi dữ liệu!"
                };
            }
            catch (Exception ex)
            {

                return new ResponseData<string>()
                {
                    IsSuccess = false,
                    Error=$"Có lỗi khi gửi dữ liệu: {ex.Message}"
                };
            }
        }
        public async Task<ResponseData<string>> RegisterHasUser(GuestRegisterUserRequest request)
        {
            try
            {

                var reponse = _client.PostAsJsonAsync("api/GuestManager/register-guest-with-user", request).GetAwaiter().GetResult();
                if (reponse.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ResponseData<string>>(await reponse.Content.ReadAsStringAsync());
                }
                return new ResponseData<string>()
                {
                    IsSuccess = false,
                    Error = $"Phát sinh lỗi không rõ khi gửi dữ liệu!"
                };
            }
            catch (Exception ex)
            {

                return new ResponseData<string>()
                {
                    IsSuccess = false,
                    Error = $"Có lỗi khi gửi dữ liệu: {ex.Message}"
                };
            }
        }
        public async Task<ResponseData<string>> ChangStatus(bool value, Guid Id)
        {
            try
            {
                DeleteRequest<Guid> request = new DeleteRequest<Guid>()
                {
                    ID = Id,
                    IsDelete = value,
                };
				var reponse =  _client.PostAsJsonAsync("api/GuestManager/change-status",request).GetAwaiter().GetResult();
                if (reponse.IsSuccessStatusCode)
                {
					var data = JsonConvert.DeserializeObject<ResponseData<string>>( await reponse.Content.ReadAsStringAsync());
					return data;
				}
				return new ResponseData<string>()
				{
					IsSuccess = false,
					Error = $"Có lỗi khi truy cập máy chủ"
				};
			}
            catch (Exception ex)
            {

                return new ResponseData<string>()
                {
                    IsSuccess = false,
                    Error=$"Có lỗi thay đổi dữ liệu: {ex.Message}"
                };
            }
        }
    }
}
