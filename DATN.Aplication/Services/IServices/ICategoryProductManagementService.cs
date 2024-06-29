using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.CategoryProduct;

namespace DATN.Aplication.Services.IServices
{
    public interface ICategoryProductManagementService
    {
        public Task<ResponseData<string>> CreateCategory(CreateCategoryProductView categoryView);
        public Task<ResponseData<string>> UpdateCategory(CreateCategoryProductView categoryView);
        public Task<ResponseData<string>> RemoveCategory(int id);
        public Task<ResponseData<string>> Active(int id);
        public Task<ResponseData<List<CategoryProductView>>> ListCategory();
    }
}