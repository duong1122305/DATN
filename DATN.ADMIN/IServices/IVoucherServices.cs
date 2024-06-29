using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.ADMIN.IServices
{
    public interface IVoucherServices
    {
        Task<ResponseData<List<VoucherView>>> GetAll();
        Task<ResponseData<string>> UpdateVoucher(VoucherView voucherView);
        Task<ResponseData<string>> CreateVoucher(VoucherView voucherView);
        Task<ResponseData<string>> ChangeStatusVoucher(int id);

    }
}
