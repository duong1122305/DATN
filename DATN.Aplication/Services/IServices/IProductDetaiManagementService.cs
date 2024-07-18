using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Product;
using DATN.ViewModels.DTOs.ProductDetail;

namespace DATN.Aplication.Services.IServices
{
    public interface IProductDetaiManagementService
    {
        public Task<ResponseData<string>> CreateProductDetail(CreateProductDetaiView productView);
        public Task<ResponseData<string>> UpdateProductDetail(CreateProductDetaiView productView);
        public Task<ResponseData<string>> RemoveProductDetail(int id);
        public Task<ResponseData<string>> ActiveProductDetail(int id);
        public Task<ResponseData<List<ProductDetaiView>>> ListProductDetail();
        public Task<ResponseData<List<ProductDetaiView>>> ListProductDetailForProduct(int id);
        //public Task<ResponseData<List<ProductDetaiView>>> ListProductDetailForProduct(int id);
    }
}