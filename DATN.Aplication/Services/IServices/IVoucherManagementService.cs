using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.Aplication.Services.IServices
{
    public interface IVoucherManagementService
    {
        public Task<ResponseData<string>> CreateVoucher(VoucherView voucherView);
        public Task<ResponseData<string>> UpdateVoucher(VoucherView voucherView);
        public Task<ResponseData<List<VoucherView>>> GetAllVoucher();
        public Task<ResponseData<string>> ExpiresVoucher(int id);
        public Task<ResponseData<List<VoucherView>>> GetAllVoucherCanApply(double totalPrice);
    }
}