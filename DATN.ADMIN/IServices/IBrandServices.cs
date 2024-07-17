using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Brand;

namespace DATN.ADMIN.IServices
{
    public interface IBrandServices
    {
        Task<ResponseData<string>> CreateBrand(BrandView brandView);
        Task<ResponseData<string>> UpdateBrand(BrandView brandView);
        Task<ResponseData<string>> RemoveBrand(int id);
        Task<ResponseData<string>> Active(int id);
        Task<ResponseData<List<BrandView>>> ListBrand();
    }
}
