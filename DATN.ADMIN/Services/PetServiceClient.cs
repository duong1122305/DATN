using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Pet;
using Newtonsoft.Json;

namespace DATN.ADMIN.Services
{
    public class PetServiceClient : IPetServiceClient
    {
        private readonly HttpClient _client;
        public PetServiceClient(HttpClient client)
        {
            _client = client;

        }

        public async Task<ResponseData<string>> CreatePet(PetCreateUpdate petVM)
        {
            try
            {

                var reponse = _client.PostAsJsonAsync("/api/PetManager/create-pet", petVM).GetAwaiter().GetResult();
                if (reponse.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<ResponseData<string>>(await reponse.Content.ReadAsStringAsync());
                    return data!;
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
                    Error = $"Có lỗi thay đổi dữ liệu: {ex.Message}"
                };
            }
        }

        public async Task<ResponseData<List<PetVM>>> GetAll()
        {
            try
            {

                var reponse = _client.GetFromJsonAsync<ResponseData<List<PetVM>>>($"/api/PetManager/get-all-pet").GetAwaiter().GetResult();
                return reponse;
            }
            catch (Exception ex)
            {

                return new ResponseData<List<PetVM>>()
                {
                    IsSuccess = false,
                    Error = $"Có lỗi khi lấy dữ liệu: {ex.Message}"
                };
            }
        }

        public async Task<ResponseData<List<PetVM>>> GetPetByGuestId(Guid guestId)
        {
            try
            {

                var reponse = _client.GetFromJsonAsync<ResponseData<List<PetVM>>>($"/api/PetManager/get-pet-by-guest?id={guestId}").GetAwaiter().GetResult();
                return reponse;
            }
            catch (Exception ex)
            {

                return new ResponseData<List<PetVM>>()
                {
                    IsSuccess = false,
                    Error = $"Có lỗi khi lấy dữ liệu: {ex.Message}"
                };
            }
        }
        public async Task<ResponseData<List<PetVM>>> GetPetBySpeciesId(int id)
        {
            try
            {

                var reponse = _client.GetFromJsonAsync<ResponseData<List<PetVM>>>($"/api/PetManager/get-pet-by-species?id={id}").GetAwaiter().GetResult();
                return reponse;
            }
            catch (Exception ex)
            {

                return new ResponseData<List<PetVM>>()
                {
                    IsSuccess = false,
                    Error = $"Có lỗi khi lấy dữ liệu: {ex.Message}"
                };
            }
        }

        public async Task<ResponseData<string>> SoftDelete(DeleteRequest<int> request)
        {
            try
            {

                var reponse = _client.PostAsJsonAsync("/api/PetManager/soft-delete-pett", request).GetAwaiter().GetResult();
                if (reponse.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<ResponseData<string>>(await reponse.Content.ReadAsStringAsync());
                    return data!;
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
                    Error = $"Có lỗi thay đổi dữ liệu: {ex.Message}"
                };
            }
        }

        public async Task<ResponseData<string>> UpdatePet(PetCreateUpdate petVM)
        {
            try
            {

                var reponse = _client.PostAsJsonAsync("/api/PetManager/update-pet", petVM).GetAwaiter().GetResult();
                if (reponse.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<ResponseData<string>>(await reponse.Content.ReadAsStringAsync());
                    return data!;
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
                    Error = $"Có lỗi thay đổi dữ liệu: {ex.Message}"
                };
            }
        }
    }
}

