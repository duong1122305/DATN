using DATN.ADMIN.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;

namespace DATN.ADMIN.Services
{
    public class PetManagerServices : IPetManagerServices
    {
        private readonly HttpClient _httpClient;
        public PetManagerServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseData<List<PetType>>> GetAllTypes()
        {
            return _httpClient.GetFromJsonAsync<ResponseData<List<PetType>>>("/api/PetManager/get-types").GetAwaiter().GetResult();
        }
    }
}
