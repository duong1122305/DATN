using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Product
{
	public class ProductClient
	{
        public int Id { get; set; }
        public string NameProduct { get; set; }
        public string BrandName { get; set; }
        public string BrandDescription { get; set; }
        public string CateDetailName { get; set; }
        public string ProductDescription { get; set; }
        public string ImgUrl { get; set; }
        public List<ProductDetailClient> ListProductDetail { get; set; }
    }
}
