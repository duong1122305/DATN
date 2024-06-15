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
    }
}
