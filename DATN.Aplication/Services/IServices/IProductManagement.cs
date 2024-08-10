using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Product;

namespace DATN.Aplication.Services.IServices
{
    public interface IProductManagement
    {
        public Task<ResponseData<(List<OrderDetail>, List<ProductDetail>)>> BuyProduct(List<BuyProduct> buyProducts);
        public Task<ResponseData<BillProduct>> GetBillProduct(int id);
        public Task<ResponseData<List<ProductSelect>>> ListProductViewSale();

    }
}