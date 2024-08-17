using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;

namespace DATN.ADMIN.Services
{
    public class EmployeeScheduleSer : IEmployeeScheduleSer
    {
        HttpClient _client;
        IHttpContextAccessor _httpContextAccessor;
        public EmployeeScheduleSer(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {

            _client = client;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Key"));
            _httpContextAccessor = httpContextAccessor;
        }
        //thêm ca làm cho tháng hiện tại
        public async Task<ResponseData<string>> AddSchuduleToMonth()
        {
            var res = await _client.GetFromJsonAsync<ResponseData<string>>("/api/UserLogin/Create-WorkShift-For-CurrentMonth");
            return res;
        }

        public async Task<ResponseData<string>> AddShuduleMonth()
        {
            var repon = await _client.GetFromJsonAsync<ResponseData<string>>("api/UserLogin/them-ca-for-all-staff");
            return repon;
        }

        public async Task<ResponseData<string>> changgeShift(ChangeShiftView changeShiftView)
        {
            var res = await _client.PostAsJsonAsync<ChangeShiftView>("/api/UserLogin/Change-Shift", changeShiftView);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await res.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<ResponseData<List<ScheduleView>>> Create(ScheduleView scheduleView)
        {
            throw new NotImplementedException();
        }
        // lấy tất cả lịch làm việc của nhân viên
        public async Task<ResponseData<List<ScheduleView>>> GetAll()
        {
            var repon = await _client.GetFromJsonAsync<ResponseData<List<ScheduleView>>>("api/get-all-ca-lam");
            return repon;
        }
        //thêm ca nhân viên cho tháng hiện tại
        public async Task<ResponseData<string>> InsertEmployeeCurrentMonth(List<string> lstStaff, int idShift)
        {
            var lst = await _client.PostAsJsonAsync<List<string>>($"api/UserLogin/Create-Shift-For-staff-in-CurrentMonth?shift={idShift}", lstStaff);
            var result = JsonConvert.DeserializeObject<ResponseData<string>>(await lst.Content.ReadAsStringAsync());
            return result;
        }

        //nhân viên trong ca
        public async Task<ResponseData<List<NumberOfScheduleView>>> ListStaffOfDay(int id, DateTime workDate)
        {
            var respone = await _client.GetFromJsonAsync<ResponseData<List<NumberOfScheduleView>>>($"api/UserLogin/Get-List-Staff-Work-in-Day?shift={id}&workdate={workDate.Year}-{workDate.Month}-{workDate.Day}");
            return respone;
        }

        //list thông tin nhân viên trống ca làm
        public async Task<ResponseData<List<UserInfView>>> lstUsrInffor(int idUser, DateTime workDate, string role)
        {
            var respone = await _client.GetFromJsonAsync<ResponseData<List<UserInfView>>>($"api/UserLogin/Get-List-Staff-Not-Working-in-Day?shiftId={idUser}&workdate={workDate.Year}-{workDate.Month}-{workDate.Day}&&role={role}");
            return respone;
        }

        public Task<ScheduleView> UpdateUser(ScheduleView scheduleView)
        {
            throw new NotImplementedException();
        }
    }
}
