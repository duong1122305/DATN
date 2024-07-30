using DATN.ViewModels.Common;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace DATN.ViewModels.DTOs.Payment
{
    public class PaymentRequest
    {
        public PaymentRequest()
        {
        }
        public static async Task<ResponseData<ResponseMomo>> sendPaymentRequest(string endpoint, string postJsonString)
        {

            try
            {
                HttpClient client = new HttpClient();
                StringContent content = new StringContent(postJsonString, Encoding.UTF8, "application/json");
                var request = await client.PostAsync(endpoint, content);
                var response = JsonConvert.DeserializeObject<ResponseMomo>(await request.Content.ReadAsStringAsync());
                return new ResponseData<ResponseMomo> { IsSuccess = true, Data = response };

            }
            catch (WebException e)
            {
                return new ResponseData<ResponseMomo> { IsSuccess = false, Error = e.Message };
            }
        }
    }
}
