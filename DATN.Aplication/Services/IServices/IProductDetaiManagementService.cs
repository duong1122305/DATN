using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Product;
using DATN.ViewModels.DTOs.ProductDetail;

namespace DATN.Aplication.Services.IServices
{
    public interface IProductDetaiManagementService
    {
        public Task<ResponseData<string>> CreateProduct(CreateProductView productView);
        public Task<ResponseData<string>> UpdateProduct(CreateProductView productView);
        public Task<ResponseData<string>> RemoveProduct(int id);
        public Task<ResponseData<string>> ActiveProduct(int id);
        public Task<ResponseData<List<ProductDetaiView>>> ListProduct();
    }
}