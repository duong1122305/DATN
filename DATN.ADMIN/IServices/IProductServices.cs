using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Product;

namespace DATN.ADMIN.IServices
{
    public interface IProductServices
    {
        Task<ResponseData<string>> CreateProduct(CreateProductView productView);
        Task<ResponseData<string>> UpdateProduct(CreateProductView productView);
        Task<ResponseData<string>> RemoveProduct(int id);
        Task<ResponseData<string>> ActiveProduct(int id);
        Task<ResponseData<List<ProductView>>> ListProduct();

        // product details
        Task<ResponseData<string>> CreateProductDetails(CreateProductView productView);
        Task<ResponseData<string>> UpdateProductDetails(CreateProductView productView);
        Task<ResponseData<string>> RemoveProductDetails(int id);
        Task<ResponseData<string>> ActiveProductDetails(int id);
        Task<ResponseData<List<ProductView>>> ListProductDetails();
    }
}
