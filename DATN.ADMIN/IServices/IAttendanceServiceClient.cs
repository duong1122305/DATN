using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Attendace;

namespace DATN.ADMIN.IServices
{
    public interface IAttendanceServiceClient
    {
        Task<ResponseData<List<Shift>>> GetShiftNow();
        Task<ResponseData<List<AttendanceViewModel>>> ListAttendanceToday(int shiftID);
    }

}