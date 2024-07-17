using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int IdCategoryProduct { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public int IdBrand { get; set; }
        public virtual Brand Brands { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual CategoryDetails CategoryDetails { get; set; }
    }
}
