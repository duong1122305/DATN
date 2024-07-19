using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Product
{
    public class ProductDetailView
    {
        public int IdProductDetail { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int SelectQuantityProduct { get; set; } = 1;
        public int? IdBooking { get; set; }
    }
}
