using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Category;
using DATN.ViewModels.DTOs.CategoryProduct;

namespace DATN.ADMIN.IServices
{
    public interface ICategoryServices
    {
        Task<ResponseData<string>> CreateCategory(CategoryView categoryView);
        Task<ResponseData<string>> UpdateCategory(CategoryView categoryView);
        Task<ResponseData<string>> RemoveCategory(int id);
        Task<ResponseData<List<CategoryView>>> ListCategory();
        Task<ResponseData<string>> ActiveCategory(int id);
        //cateproduct details
        Task<ResponseData<List<CategoryProductView>>> ListCategoryProduct();
        Task<ResponseData<string>> CreateCategoryProduct(CreateCategoryProductView categoryView);
        Task<ResponseData<string>> UpdateCategoryProduct(CreateCategoryProductView categoryView);
        Task<ResponseData<string>> RemoveCategoryProduct(int id);
        Task<ResponseData<string>> ActiveCategoryProduct(int id);

    }
}
