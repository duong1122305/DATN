using DATN.ADMIN.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Attendace;

namespace DATN.ADMIN.Services
{
    public class AttendanceServiceClient : IAttendanceServiceClient
    {
        private readonly HttpClient _client;

        public AttendanceServiceClient(HttpClient client)
        {
            _client = client;
        }
        public async Task<ResponseData<List<Shift>>> GetShiftNow()
        {
            return  _client.GetFromJsonAsync<ResponseData<List<Shift>>>("/api/Attendace/select_shift-data").GetAwaiter().GetResult()!;
        }

        public async Task<ResponseData<List<AttendanceViewModel>>> ListAttendanceToday(int shiftID)
        {
            return _client.GetFromJsonAsync<ResponseData<List<AttendanceViewModel>>>($"api/Attendace?shiftID={shiftID}").GetAwaiter().GetResult()!;
        }
    }
}
