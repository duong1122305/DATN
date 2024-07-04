using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Attendace;

namespace DATN.Aplication.Services.IServices
{
    public interface IAttendanteMangarService
    {
        Task<ResponseData<List<Shift>>> GetShiftNow();
        Task<ResponseData<List<AttendanceViewModel>>> ListAttendanceToday(int shiftID);
    }
}