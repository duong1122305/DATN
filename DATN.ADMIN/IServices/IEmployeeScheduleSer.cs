using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.ADMIN.IServices
{
    public interface IEmployeeScheduleSer

    {
        Task<ResponseData<List<ScheduleView>>> Create(ScheduleView scheduleView);
        Task<ResponseData<List<ScheduleView>>> GetAll();
        Task<ScheduleView> UpdateUser(ScheduleView scheduleView);
        Task<ResponseData<List<NumberOfScheduleView>>> ListStaffOfDay(int id, DateTime workDate);
        Task<ResponseData<List<UserInfView>>> lstUsrInffor(int idUser, DateTime workDate, string role);

        Task<ResponseData<string>> changgeShift(ChangeShiftView changeShiftView);
        //thêm 1 tháng sau
        Task<ResponseData<string>> AddShuduleMonth();
        //thêm tháng hiện tại
        Task<ResponseData<string>> AddSchuduleToMonth();
        Task<ResponseData<string>> InsertEmployeeCurrentMonth(List<string> lstStaff, int idShift);
    }
}
