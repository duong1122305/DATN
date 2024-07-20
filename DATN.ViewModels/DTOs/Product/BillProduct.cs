using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Product
{
    public class BillProduct
    {
        public double TotalPrice { get; set; }
        public List<ProductDetailView> ListProductDetail {  get; set; }
    }
}
