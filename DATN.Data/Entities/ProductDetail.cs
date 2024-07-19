using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
    public class ProductDetail
    {
        public int Id { get; set; }
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Product Product { get; set; }
        public virtual PetType PetType { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ImageProduct> ImageProducts { get; set; }
    }
}
