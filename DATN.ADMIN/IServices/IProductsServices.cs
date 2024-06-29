using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.CategoryProduct;

namespace DATN.ADMIN.IServices
{
    public interface IProductsServices
    {
        Task<ResponseData<List<CategoryProductView>>> ListCategoryProduct();
        Task<ResponseData<string>> CreateCategoryProduct(CreateCategoryProductView categoryView);
        Task<ResponseData<string>> UpdateCategoryProduct(CreateCategoryProductView categoryView);
        Task<ResponseData<string>> RemoveCategoryProduct(int id);
        Task<ResponseData<string>> ActiveCategoryProduct(int id);
    }
}
