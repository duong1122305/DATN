using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Attendace;

namespace DATN.Aplication.Services.IServices
{
    public interface IAttendanteMangarService
    {
        Task<ResponseData<List<Shift>>> GetShiftNow();
        Task<ResponseData<List<AttendanceViewModel>>> ListAttendanceToday(int shiftID);
        Task<ResponseData<string>> CheckIn(int scheduleId, int attendanceID, bool isCheckin, Guid userId);
        Task<ResponseData<string>> CheckOut(int attendanceID, bool isCheckout, Guid userId);
        Task<ResponseData<List<ShiftVM>>> GetPersonalShift(Guid id);
    }
}