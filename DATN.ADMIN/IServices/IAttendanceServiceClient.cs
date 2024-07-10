using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Attendace;

namespace DATN.ADMIN.IServices
{
    public interface IAttendanceServiceClient
    {
        Task<ResponseData<List<Shift>>> GetShiftNow();
        Task<ResponseData<List<AttendanceViewModel>>> ListAttendanceToday(int shiftID);
        Task<ResponseData<string>> CheckIn(int scheduleId, int attendanceID, bool isCheckin);
        Task<ResponseData<string>> CheckOut(int attendanceID, bool isCheckout);
        Task<ResponseData<string>> CheckInOutQR(int workShiftID, Guid UserID, bool isCheckIn, string note = "");
        Task<ResponseData<List<ShiftVM >>> GetShiftQR(Guid id);
        Task<ResponseData<string>> GetTodayCode();
        Task<ResponseData<string>> CheckCode(string code);
    }

}