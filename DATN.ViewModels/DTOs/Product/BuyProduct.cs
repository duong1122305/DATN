using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Product
{
    public class BuyProduct
    {
        public int IdBooking { get; set; }
        public int IdProductDetail { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
