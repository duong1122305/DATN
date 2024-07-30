using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Category;

namespace DATN.Aplication.Services.IServices
{
    public interface ICategoryManagementService
    {
        public Task<ResponseData<string>> CreateCategory(CategoryView categoryView);
        public Task<ResponseData<string>> UpdateCategory(CategoryView categoryView);
        public Task<ResponseData<string>> RemoveCategory(int id);
        public Task<ResponseData<string>> Active(int id);
        public Task<ResponseData<List<CategoryView>>> ListCategory();
    }
}
