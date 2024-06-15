using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.Aplication.Services.IServices
{
    public interface IEmployeeScheduleManagementService
    {
        public Task<ResponseData<string>> InsertEmployeeNextMonthCompareCurrentMonth(List<string> listUser, int shift);
        public Task<ResponseData<List<ScheduleView>>> GetUserInOneMonth(int month, int year);
        public Task<ResponseData<List<ScheduleView>>> GetScheduleForShift(int shift);
        public Task<ResponseData<List<ScheduleView>>> GetAll();
        public Task<ResponseData<List<ScheduleView>>> GetScheduleFromMonthToMonth(ScheduleMonthToMonthView scheduleMonthToMonthView);
        public Task<ResponseData<List<NumberOfScheduleView>>> GetListStaffInDay(int shift, DateTime workdate);
        public Task<ResponseData<List<UserInfView>>> ListStaffNotWorkingInDay(int shiftId,DateTime workDate);
        public Task<ResponseData<string>> ChangeShiftStaffToStaff(ChangeShiftView changeShiftView);


    }
}