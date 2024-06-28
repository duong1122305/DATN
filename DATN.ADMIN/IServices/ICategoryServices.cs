using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Category;

namespace DATN.ADMIN.IServices
{
    public interface ICategoryServices
    {
        Task<ResponseData<string>> CreateCategory(CategoryView categoryView);
        Task<ResponseData<string>> UpdateCategory(CategoryView categoryView);
        Task<ResponseData<string>> RemoveCategory(int id);
        Task<ResponseData<List<CategoryView>>> ListCategory();
        Task<ResponseData<string>> ActiveCategory(int id);
    }
}
