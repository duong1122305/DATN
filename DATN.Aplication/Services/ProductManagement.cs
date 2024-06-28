using DATN.Data.Entities;
using DATN.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
    public class ProductManagement
    {
        IUnitOfWork _unitOfWork;
        public ProductManagement(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<ResponseData<string>> CreateProduct(ProductView productView)
        {
            return null;
        }
    }
}
