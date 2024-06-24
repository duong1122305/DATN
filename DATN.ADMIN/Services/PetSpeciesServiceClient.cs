using DATN.ADMIN.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Guest;
using DATN.ViewModels.DTOs.PetSpecies;
using Newtonsoft.Json;

namespace DATN.ADMIN.Services
{
	public class PetSpeciesServiceClient : IPetSpeciesServiceClient
	{
		private readonly HttpClient _client;

		public PetSpeciesServiceClient(HttpClient httpClient)
        {
				_client = httpClient;
		}

		public async Task<ResponseData<string>> Create(PetSpeciesCreateUpdate request)
		{
			try
			{

				var reponse = _client.PostAsJsonAsync("/api/PetSpecies/create-species", request).GetAwaiter().GetResult();
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

		public async Task<ResponseData<PetSpeciesVM>> FindPetSpeciesByID(int id)
		{
			try
			{

				var reponse =  _client.GetFromJsonAsync<ResponseData<PetSpeciesVM>>("/api/PetSpecies/get-by-id-species").GetAwaiter().GetResult();
				return reponse;
			}
			catch (Exception ex)
			{

				return new ResponseData<PetSpeciesVM>()
				{
					IsSuccess = false,
					Error = $"Có lỗi khi lấy dữ liệu: {ex.Message}"
				};
			}
		}

		public async Task<ResponseData<List<PetSpeciesVM>>> GetAll()
		{
			try
			{

				var reponse = _client.GetFromJsonAsync<ResponseData<List<PetSpeciesVM>>>("/api/PetSpecies/get-all").GetAwaiter().GetResult();
				return reponse;
			}
			catch (Exception ex)
			{

				return new ResponseData<List<PetSpeciesVM>>()
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
				
				var reponse = _client.PostAsJsonAsync("/api/PetSpecies/delete-species", request).GetAwaiter().GetResult();
				if (reponse.IsSuccessStatusCode)
				{
					var data = JsonConvert.DeserializeObject<ResponseData<string>>(await reponse.Content.ReadAsStringAsync());
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
					Error = $"Có lỗi thay đổi dữ liệu: {ex.Message}"
				};
			}
		}

		public async Task<ResponseData<string>> Update(PetSpeciesCreateUpdate request)
		{
			try
			{

				var reponse = _client.PostAsJsonAsync("/api/PetSpecies/update-species", request).GetAwaiter().GetResult();
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
