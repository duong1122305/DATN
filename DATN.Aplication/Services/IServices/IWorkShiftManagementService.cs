using DATN.ViewModels.Common;

namespace DATN.Aplication.Services.IServices
{
    public interface IWorkShiftManagementService
    {
        public Task<ResponseData<string>> InsertWorkShiftNextMonthCompareCurrentMonth();
        public Task<ResponseData<string>> InsertWorkShiftCurrentMonth();
    }
}