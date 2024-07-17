using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Product;

namespace DATN.Aplication.Services
{
    public interface IProductManagement
    {
        public Task<ResponseData<string>> BuyProduct(List<BuyProduct> buyProducts);
        public Task<ResponseData<BillProduct>> GetBillProduct(int id);

    }
}