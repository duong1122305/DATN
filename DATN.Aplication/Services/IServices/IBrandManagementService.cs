using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Brand;

namespace DATN.Aplication.Services.IServices
{
    public interface IBrandManagementService
    {
        public Task<ResponseData<string>> CreateBrand(BrandView brandView);
        public Task<ResponseData<string>> UpdateCategory(BrandView brandView);
        public Task<ResponseData<string>> RemoveCategory(int id);
        public Task<ResponseData<string>> Active(int id);
        public Task<ResponseData<List<BrandView>>> ListBrand();

    }
}