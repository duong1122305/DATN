using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.Aplication.Services.IServices
{
    public interface IShiftManagementService
    {
        public Task<ResponseData<string>> CreateShift(ShiftView shift);
        public Task<ResponseData<string>> UpdateShift(ShiftView shift,int id);
        public Task<ResponseData<List<Shift>>> GetListShift();
    }
}