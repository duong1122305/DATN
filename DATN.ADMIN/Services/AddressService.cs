using Azure;
using DATN.ADMIN.IServices;
using DATN.ViewModels.Common.Location;
using Newtonsoft.Json;

namespace DATN.ADMIN.Services
{
    public class AddressService:IAddressService
    {
        HttpClient _client;
        public AddressService(HttpClient client)
        {
            _client = client;
        }
        public async Task<List<DataAdress>> GetProvinces()
        {
            try
            {
                var response = await _client.GetStringAsync("https://esgoo.net/api-tinhthanh/1/0.htm");

                var dataAll = JsonConvert.DeserializeObject<AddressAPIResponse>(response);
                var provinces = dataAll.data;
                if (provinces==null)
                {
                    return new List<DataAdress>();
                }
                return provinces;
            }
            catch (Exception)
            {
                return new List<DataAdress>();
               
            }
        }

        public  async Task<List<DataAdress>> GetDistricts(string provinceCode)
        {
           
            try
            {
                var response = await _client.GetStringAsync($"https://esgoo.net/api-tinhthanh/2/{provinceCode}.htm");
                var dataAll = JsonConvert.DeserializeObject<AddressAPIResponse>(response);
                var districts = dataAll.data;

                if (districts == null)
                {
                    return new List<DataAdress>();
                }
                return districts;
            }
            catch (Exception)
            {
                return new List<DataAdress>();

            }
        }
        public  async Task<List<DataAdress>> GetWards(string districtCode)
        {
            try
            {
                var response = await _client.GetStringAsync($"https://esgoo.net/api-tinhthanh/3/{districtCode}.htm");
                var dataAll = JsonConvert.DeserializeObject<AddressAPIResponse>(response);
                var wards = dataAll.data;

                if (wards == null)
                {
                    return new List<DataAdress>();
                }
                return wards;
            }
            catch (Exception)
            {
                return new List<DataAdress>();

            }
        }
        public async Task<string>GetFullNameByWard(string addressCode)
        {
            try
            {
                var arrData= addressCode.Split('|');
                var response= await _client.GetStringAsync($"https://esgoo.net/api-tinhthanh/5/{arrData[0]}.htm");
				var dataAll = JsonConvert.DeserializeObject<AddressAPIResponse>(response);
                return dataAll.data.First().full_name;
			}
            catch (Exception)
            {
                return null;
                
            }
                 
        }   
        public async Task<DataAdress> GetFullCodeByWard(string addressCode)
        {
            try
            {
                var arrData= addressCode.Split('|');
                var response= await _client.GetStringAsync($"https://esgoo.net/api-tinhthanh/5/{arrData[0]}.htm");
				var dataAll = JsonConvert.DeserializeObject<AddressAPIResponse>(response);
                return dataAll.data.First();
                // id tinh quan 
			}
            catch (Exception)
            {
                return null;
                
            }
                 
        }
    }
}
