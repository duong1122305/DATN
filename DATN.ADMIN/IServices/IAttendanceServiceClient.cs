using DATN.Data.Entities;
using DATN.Utilites;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Attendace;

namespace DATN.ADMIN.IServices
{
    public interface IAttendanceServiceClient
    {
        Task<ResponseData<List<Shift>>> GetShiftNow();
        Task<ResponseData<string>> CheckLate(int shiftID, bool isCheckin);

        Task<ResponseData<List<AttendanceViewModel>>> ListAttendanceToday(int shiftID);
        Task<ResponseData<string>> CheckIn(int scheduleId, int attendanceID, bool isCheckin, Guid userId);
        Task<ResponseData<string>> CheckOut(int scheduleId, int attendanceID, bool isCheckout, Guid userId);
        Task<ResponseData<AttendancePersonal>> GetPersonalShift(Guid id);
        Task<ResponseData<string>> CheckOutPersonal(AttendancePersonal atendance);
        Task<ResponseData<string>> CheckInPersonal(AttendancePersonal atendance);
        Task<ResponseData<List<AttendanceMonth>>> GetAllAttandanceMonth(int month = 0, int year = 0, int isDelete = -1);
        Task<ResponseData<ListPerAttenMonth>> GetAttandanceMonth(Guid idUser, int month = 0, int year = 0, int TypeAttendance = Contant.AllAttendance);

    }

}