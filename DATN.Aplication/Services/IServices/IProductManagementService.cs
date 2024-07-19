using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Product;

namespace DATN.Aplication.Services.IServices
{
    public interface IProductManagementService
    {
        public Task<ResponseData<string>> CreateProduct(CreateProductView productView);
        public Task<ResponseData<string>> UpdateProduct(CreateProductView productView);
        public Task<ResponseData<string>> RemoveProduct(int id);
        public Task<ResponseData<string>> ActiveProduct(int id);
        public Task<ResponseData<List<ProductView>>> ListProduct();
    }
}