using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.Aplication.Services
{
    public interface IVoucherManagementService
    {
        public Task<ResponseData<string>> CreateVoucher(VoucherView voucherView);
        public Task<ResponseData<string>> UpdateVoucher(VoucherView voucherView);
        public Task<ResponseData<List<VoucherView>>> GetAllVoucher();
    }
}