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

        public Task<ScheduleView> UpdateUser(ScheduleView scheduleView)
        {
            throw new NotImplementedException();
        }
    }
}
