using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.Aplication.Services.IServices
{
    public interface IEmployeeScheduleManagementService
    {
        public Task<ResponseData<string>> InsertEmployeeNextMonthCompareCurrentMonth(List<string> listUser, int shift);
        public Task<ResponseData<List<ScheduleView>>> GetUserInOneMonth(int month, int year);
        // nếu sai hay xoá dòng này
        public Task<ResponseData<List<ScheduleView>>> GetAllCa();
    }
}