using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Product
{
    public class ProductSelect
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string CateName { get; set; }
        public string LinkImg { get; set; }
        public List<ProductDetailView> ListProductDetail { get; set; }
    }
}
