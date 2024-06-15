using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.ADMIN.Services
{
    public class EmployeeScheduleSer : IEmployeeScheduleSer
    {
        HttpClient _client;
        public EmployeeScheduleSer(HttpClient client)
        {

            _client = client;

        }

        public async Task<ResponseData<List<ScheduleView>>> Create(ScheduleView scheduleView)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<List<ScheduleView>>> GetAll()
        {
            var repon = await _client.GetFromJsonAsync<ResponseData<List<ScheduleView>>>("api/get-all-ca-lam");
            return repon;
        }

        public async Task<ResponseData<List<NumberOfScheduleView>>> ListStaffOfDay(int id, DateTime workDate)
        {
            var respone = await _client.GetFromJsonAsync<ResponseData<List<NumberOfScheduleView>>>($"api/UserLogin/Get-List-Staff-Work-in-Day?shift={id}&workdate={workDate.Year}-{workDate.Month}-{workDate.Day}");
            return respone;
        }

        public async Task<ResponseData<List<UserInfView>>> lstUsrInffor(int idUser, DateTime workDate)
        {
            var respone = await _client.GetFromJsonAsync<ResponseData<List<UserInfView>>>($"api/UserLogin/Get-List-Staff-Not-Working-in-Day?shiftId={idUser}&workdate={workDate.Year}-{workDate.Month}-{workDate.Day}");
            return respone;
        }

        public Task<ScheduleView> UpdateUser(ScheduleView scheduleView)
        {
            throw new NotImplementedException();
        }
    }
}
