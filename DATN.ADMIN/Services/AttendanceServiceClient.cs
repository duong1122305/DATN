using Azure.Core;
using DATN.ADMIN.IServices;
using DATN.Data.Entities;
using DATN.Utilites;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Attendace;
using DATN.ViewModels.DTOs.Pet;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DATN.ADMIN.Services
{
	public class AttendanceServiceClient : IAttendanceServiceClient
	{
		private readonly HttpClient _client;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AttendanceServiceClient(HttpClient client, IHttpContextAccessor httpContextAccessor)
		{
			_client = client;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<ResponseData<string>> CheckIn(int scheduleId, int attendanceID, bool isCheckin, Guid id)
		{


			return _client.GetFromJsonAsync<ResponseData<string>>($@"/api/Attendace/check-in-hand?scheduleId={scheduleId}&attendanceID={attendanceID}&isCheckin={isCheckin}&userId={id}").GetAwaiter().GetResult()!;


		}

		public async Task<ResponseData<string>> CheckOut(int scheduleId, int attendanceID, bool isCheckout, Guid id)
		{

			return _client.GetFromJsonAsync<ResponseData<string>>($@"/api/Attendace/check-out-hand?scheduleId={scheduleId}&attendanceID={attendanceID}&isCheckout={isCheckout}&userId={id}").GetAwaiter().GetResult()!;
		}

		public async Task<ResponseData<List<Shift>>> GetShiftNow()
		{
			return _client.GetFromJsonAsync<ResponseData<List<Shift>>>("/api/Attendace/select_shift-data").GetAwaiter().GetResult()!;
		}

		public async Task<ResponseData<List<AttendanceViewModel>>> ListAttendanceToday(int shiftID)
		{
			var key = _httpContextAccessor.HttpContext.Session.GetString("Key");
			Guid idUser;
			if (key != null)
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var jwtSecurityToken = tokenHandler.ReadJwtToken(key);
				// var listClaims = jwtSecurityToken.Claims.ToArray();
				idUser = Guid.Parse(jwtSecurityToken.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value);

			}
			return _client.GetFromJsonAsync<ResponseData<List<AttendanceViewModel>>>($"api/Attendace?shiftID={shiftID}").GetAwaiter().GetResult()!;
		}


		public async Task<ResponseData<AttendancePersonal>> GetPersonalShift(Guid id)
		{
			return _client.GetFromJsonAsync<ResponseData<AttendancePersonal>>($"/api/Attendace/check-attendance-personal?id={id}").GetAwaiter().GetResult()!;
		}



		public async Task<ResponseData<string>> CheckOutPersonal(AttendancePersonal atendance)
		{

			var reponse = _client.PostAsJsonAsync("api/Attendace/check-out-per", atendance).GetAwaiter().GetResult();
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

		public async Task<ResponseData<string>> CheckInPersonal(AttendancePersonal atendance)
		{
			var reponse = _client.PostAsJsonAsync("api/Attendace/check-in-per", atendance).GetAwaiter().GetResult();
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

		public async Task<ResponseData<string>> CheckLate(int shiftID, bool isCheckin)
		{
			return _client.GetFromJsonAsync<ResponseData<string>>($"/api/Attendace/check-shift?shiftID={shiftID}&isCheckin={isCheckin}").GetAwaiter().GetResult()!;
		}

		public async Task<ResponseData<List<AttendanceMonth>>> GetAllAttandanceMonth(int month = 0, int year = 0, int isDelete = -1)
		{
			return _client.GetFromJsonAsync<ResponseData<List<AttendanceMonth>>>($"/api/Attendace/GetAllAttendance?month={month}&year={year}&isDelete={isDelete}").GetAwaiter().GetResult()!;
		}

		public async Task<ResponseData<ListPerAttenMonth>> GetAttandanceMonth(Guid idUser, int month = 0, int year = 0,int typeAttendance= Contant.AllAttendance)
		{
			return _client.GetFromJsonAsync<ResponseData<ListPerAttenMonth>>($"/api/Attendace/get-attendance-month-by-user?idUser={idUser}&month={month}&year={year}&typeattendance={typeAttendance}").GetAwaiter().GetResult()!;
		}
	}

}
