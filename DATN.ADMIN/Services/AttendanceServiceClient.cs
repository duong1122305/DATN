using DATN.ADMIN.IServices;
using DATN.Data.Entities;
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

		public async Task<ResponseData<string>> CheckIn(int scheduleId, int attendanceID, bool isCheckin)
		{
			

				return  _client.GetFromJsonAsync<ResponseData<string>>($@"/api/Attendace/check-in-hand?scheduleId={scheduleId}&attendanceID={attendanceID}&isCheckin={isCheckin}").GetAwaiter().GetResult()!;

        
		}

		public async Task<ResponseData<string>> CheckOut(int attendanceID, bool isCheckout)
		{

            return _client.GetFromJsonAsync<ResponseData<string>>($@"/api/Attendace/check-out-hand?attendanceID={attendanceID}&isCheckout={isCheckout}").GetAwaiter().GetResult()!;
        }

		public async Task<ResponseData<List<Shift>>> GetShiftNow()
        {
            return  _client.GetFromJsonAsync<ResponseData<List<Shift>>>("/api/Attendace/select_shift-data").GetAwaiter().GetResult()!;
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
		public async Task<ResponseData<string>> GetTodayCode()
		{
			return _client.GetFromJsonAsync<ResponseData<string>>("/api/Attendace/getToDayCode").GetAwaiter().GetResult()!;
		}

        public async Task<ResponseData<string>> CheckInOutQR(int workShiftID, Guid UserID, bool isCheckIn, string note = "")
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<List<ShiftVM>>> GetShiftQR(Guid id)
        {
            return _client.GetFromJsonAsync<ResponseData<List<ShiftVM>>>($"/api/Attendace/check-shift-qr?id={id}").GetAwaiter().GetResult()!;
        }



        public async Task<ResponseData<string>> CheckCode(string code)
        {
            throw new NotImplementedException();
        }

     
    }
}
